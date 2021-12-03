using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportManager : MonoBehaviour
{
    public static TransportManager Instance;

    [HideInInspector] public List<Utils.Transport> Transports;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetTeamTransportsResponseEvent += OnGetTransportsResponseReceived;
        EventManager.Instance.OnTransportStateChangedResponseEvent += OnTransportStateChangedResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetTeamTransportsResponseEvent -= OnGetTransportsResponseReceived;
        EventManager.Instance.OnTransportStateChangedResponseEvent -= OnTransportStateChangedResponseReceived;
    }

    private void OnGetTransportsResponseReceived(GetTeamTransportsResponse getTeamTransportsResponse)
    {
        Transports = getTeamTransportsResponse.myTeamTransports;
        TransportsController.Instance.Initialize();
    }

    private void OnTransportStateChangedResponseReceived(TransportStateChangedResponse transportStateChangedResponse)
    {
        Utils.Transport transport = GetTransportById(transportStateChangedResponse.transport.id);
        switch (transportStateChangedResponse.transport.transportState)
        {
            case Utils.TransportState.IN_WAY:
                if (transport == null)
                {
                    Transports.Add(transportStateChangedResponse.transport);
                    TransportsController.Instance.AddInWay(transportStateChangedResponse.transport);
                }
                else
                {
                    transport.transportState = Utils.TransportState.IN_WAY;
                    TransportsController.Instance.AddInWay(transport);
                }
                break;
            case Utils.TransportState.SUCCESSFUL:
                Transports.Remove(transport);
                TransportsController.Instance.AddDone(transport);
                //TODO notification or something
                break;
            case Utils.TransportState.CRUSHED:
                Transports.Remove(transport);
                TransportsController.Instance.AddCrashed(transport);
                //TODO notification or something
                break;
            case Utils.TransportState.PENDING:
                Transports.Add(transportStateChangedResponse.transport);
                break;
        }

        ApplyChangesToStorage(transportStateChangedResponse.transport);

        if (MapManager.IsInMap)
        {
            MapManager.Instance.UpdateLine(transportStateChangedResponse.transport);
        }
    }

    private Utils.Transport GetTransportById(int id)
    {
        return Transports.Find(t => t.id == id);
    }

    public List<Utils.Transport> GetTransportsByDestinationTypeAndId(Utils.TransportNodeType destinationType, int destinationId)
    {
        return Transports.FindAll(t => t.destinationType == destinationType && t.destinationId == destinationId && t.transportState == Utils.TransportState.IN_WAY);
    }

    private void ApplyChangesToStorage(Utils.Transport transport)
    {
        if (transport.transportState == Utils.TransportState.IN_WAY)
        {
            switch (transport.sourceType)
            {
                case Utils.TransportNodeType.FACTORY:
                    Utils.Storage factoryStorage = StorageManager.Instance.GetStorageByBuildingIdAndType(transport.sourceId, false);
                    if (factoryStorage != null)
                    {
                        StorageManager.Instance.ChangeStockInStorage(factoryStorage.id, transport.contentProductId, -1 * transport.contentProductAmount);
                    }
                    break;
                case Utils.TransportNodeType.DC:
                    Utils.Storage dcStorage = StorageManager.Instance.GetStorageByBuildingIdAndType(transport.sourceId, true);
                    if (dcStorage != null)
                    {
                        StorageManager.Instance.ChangeStockInStorage(dcStorage.id, transport.contentProductId, -1 * transport.contentProductAmount);
                    }
                    break;
            }
        }
        else if (transport.transportState == Utils.TransportState.SUCCESSFUL)
        {
            switch (transport.destinationType)
            {
                case Utils.TransportNodeType.FACTORY:
                    Utils.Storage factoryStorage = StorageManager.Instance.GetStorageByBuildingIdAndType(transport.destinationId, false);
                    if (factoryStorage != null)
                    {
                        StorageManager.Instance.ChangeStockInStorage(factoryStorage.id, transport.contentProductId, transport.contentProductAmount);
                    }
                    break;
                case Utils.TransportNodeType.DC:
                    Utils.Storage dcStorage = StorageManager.Instance.GetStorageByBuildingIdAndType(transport.destinationId, true);
                    if (dcStorage != null)
                    {
                        StorageManager.Instance.ChangeStockInStorage(dcStorage.id, transport.contentProductId, transport.contentProductAmount);
                    }
                    break;
            }
        }
    }

    public int CalculateTransportDuration(Vector2 sourceLocation, Vector2 destinationLocation, Utils.VehicleType vehicleType)
    {
        int transportDistance = GetTransportDistance(sourceLocation, destinationLocation, vehicleType);
        return (int)Mathf.Ceil((float)transportDistance / GameDataManager.Instance.GetVehicleByType(vehicleType).speed);
    }

    public int GetTransportDistance(Vector2 sourceLocation, Vector2 destinationLocation, Utils.VehicleType vehicleType)
    {
        double distance = (sourceLocation.x - destinationLocation.x) * (sourceLocation.x - destinationLocation.x);
        distance += (sourceLocation.y - destinationLocation.y) * (sourceLocation.y - destinationLocation.y);
        distance = Mathf.Sqrt((float) distance);
        return (int)Mathf.Ceil((float) distance * GameDataManager.Instance.GameConstants.distanceConstant * GameDataManager.Instance.GetVehicleByType(vehicleType).coefficient);
    }

    public float CalculateTransportCost(Utils.VehicleType vehicleType, int distance, int productId, int productAmount, bool hasInsurance)
    {
        float insuranceFactor = (hasInsurance ? (1 + GameDataManager.Instance.GameConstants.insuranceCostFactor) : 1);
        Utils.Vehicle transportVehicle = GameDataManager.Instance.GetVehicleByType(vehicleType);
        float vehicleCost = transportVehicle.costPerKilometer * distance * insuranceFactor;
        int productVolume = GameDataManager.Instance.GetProductById(productId).volumetricUnit * productAmount;
        int vehicleCount = (int)Mathf.Ceil((float)productVolume / transportVehicle.capacity);
        return vehicleCost * vehicleCount;
    }

    public float CalculateTransportCost(Utils.VehicleType vehicleType, int distance, int productId, int productAmount)
    {
        Utils.Vehicle transportVehicle = GameDataManager.Instance.GetVehicleByType(vehicleType);
        float vehicleCost = transportVehicle.costPerKilometer * distance;
        int productVolume = GameDataManager.Instance.GetProductById(productId).volumetricUnit * productAmount;
        int vehicleCount = (int)Mathf.Ceil((float)productVolume / transportVehicle.capacity);
        return vehicleCost * vehicleCount;
    }

    public int CalculateInWayProductsAmount(Utils.Storage storage, Utils.ProductType productType)
    {
        int amount = 0;
        if (storage.dc)
        {
            List<Utils.Transport> thisDcTransports = GetTransportsByDestinationTypeAndId(Utils.TransportNodeType.DC, storage.buildingId);
            foreach (Utils.Transport transport in thisDcTransports)
            {
                if (GameDataManager.Instance.GetProductById(transport.contentProductId).productType == productType)
                {
                    amount += transport.contentProductAmount;
                }
            }
        }
        else
        {
            List<Utils.Transport> thisWarehouseTransports = GetTransportsByDestinationTypeAndId(Utils.TransportNodeType.FACTORY, storage.buildingId);
            foreach (Utils.Transport transport in thisWarehouseTransports)
            {
                if (GameDataManager.Instance.GetProductById(transport.contentProductId).productType == productType)
                {
                    amount += transport.contentProductAmount;
                }
            }
        }

        return amount;
    }

    public int CalculateInWayProductsAmount(Utils.Storage storage, int productId)
    {
        int amount = 0;
        if (storage.dc)
        {
            List<Utils.Transport> thisDcTransports = GetTransportsByDestinationTypeAndId(Utils.TransportNodeType.DC, storage.buildingId);
            foreach (Utils.Transport transport in thisDcTransports)
            {
                if (transport.contentProductId == productId)
                {
                    amount += transport.contentProductAmount;
                }
            }
        }
        else
        {
            List<Utils.Transport> thisWarehouseTransports = GetTransportsByDestinationTypeAndId(Utils.TransportNodeType.FACTORY, storage.buildingId);
            foreach (Utils.Transport transport in thisWarehouseTransports)
            {
                if (transport.contentProductId == productId)
                {
                    amount += transport.contentProductAmount;
                }
            }
        }

        return amount;
    }
}
