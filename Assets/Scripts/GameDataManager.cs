using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public List<Utils.Team> Teams;
    [HideInInspector] public List<Utils.GameinCustomer> GameinCustomers;
    [HideInInspector] public List<Utils.Supplier> GameinSuppliers;
    [HideInInspector] public List<Utils.Product> Products;
    [HideInInspector] public List<Utils.Vehicle> Vehicles;
    [HideInInspector] public List<Utils.ProductionLineTemplate> ProductionLineTemplates;

    [HideInInspector] public List<Utils.Auction> Auctions;
    [HideInInspector] public List<Utils.Factory> Factories;

    [HideInInspector] public List<Utils.WeekDemand> CurrentWeekDemands;
    [HideInInspector] public List<Utils.WeekSupply> CurrentWeekSupplies;

    
    [HideInInspector] public List<Utils.DC> DCs;
    [HideInInspector] public Utils.GameConstants GameConstants;

    public List<Sprite> ProductSprites;

    private int _auctionCurrentRound = 0;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
        EventManager.Instance.OnGetCurrentWeekDemandsResponseEvent += OnGetCurrentWeekDemandsResponse;
        EventManager.Instance.OnGetCurrentWeekSuppliesResponseEvent += OnGetCurrentWeekSuppliesResponse;
        EventManager.Instance.OnGetAllAuctionsResponseEvent += OnGetAllAuctionsResponse;
        EventManager.Instance.OnAuctionFinishedResponseEvent += OnAuctionFinishedResponse;
        EventManager.Instance.OnGetAllActiveDcResponseEvent += OnGetAllActiveDCsResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        Teams = getGameDataResponse.teams;
        GameinCustomers = getGameDataResponse.gameinCustomers;
        GameinSuppliers = getGameDataResponse.suppliers;
        Products = getGameDataResponse.products;
        Vehicles = getGameDataResponse.vehicles;
        ProductionLineTemplates = getGameDataResponse.productionLineTemplates;

        Debug.Log(ProductionLineTemplates);

        Factories = getGameDataResponse.factories;

        GameConstants = getGameDataResponse.gameConstants;
    }

    public void OnGetAllActiveDCsResponse(GetAllActiveDcResponse getAllActiveDcResponse)
    {
        DCs = getAllActiveDcResponse.dcs;
    }

    public void OnGetCurrentWeekDemandsResponse(GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse)
    {
        CurrentWeekDemands = getCurrentWeekDemandsResponse.currentWeekDemands;

        GameinCustomersController.Instance.UpdateDemands();
    }

    public void OnGetCurrentWeekSuppliesResponse(GetCurrentWeekSuppliesResponse getCurrentWeekSuppliesResponse)
    {
        CurrentWeekSupplies = getCurrentWeekSuppliesResponse.currentWeekSupplies;

        GameinSuppliersController.Instance.UpdateSupplies();
    }
    
    public void OnGetAllAuctionsResponse(GetAllAuctionsResponse getAllAuctionsResponse)
    {
        Auctions = getAllAuctionsResponse.auctions;

        if (SceneManager.GetActiveScene().name == "MapScene")
            MapManager.Instance.UpdateAllAuctions();

        _auctionCurrentRound = 0;
        foreach(CustomDateTime customDateTime in GameConstants.AuctionRoundsStartTime)
        {
            if (DateTime.Now >= customDateTime.ToDateTime())
            {
                _auctionCurrentRound++;
            }
        }

        //TODO update auction remained time
    }

    public void OnAuctionFinishedResponse(AuctionFinishedResponse auctionFinishedResponse)
    {
        Teams = auctionFinishedResponse.teams;

        int teamId = PlayerPrefs.GetInt("TeamId");
        foreach (Utils.Team team in Teams)
        {
            if (team.id == teamId)
            {
                PlayerPrefs.SetInt("FactoryId", team.factoryId);
            }
        }

        SceneManager.UnloadSceneAsync("MapScene");
    }
    
    public Utils.Auction GetAuctionByFactoryId(int id)
    {
        return Auctions.FirstOrDefault(a => a.factoryId == id);
    }

    public void UpdateAuctionElement(Utils.Auction auction)
    {
        int index = Auctions.IndexOf(GetAuctionByFactoryId(auction.factoryId));
        if (index != -1)
        {
            Auctions[index] = auction;
        }
        else
        {
            Auctions.Add(auction);
        }

        MapManager.Instance.UpdateAllAuctions();
    }
    
    public Utils.Factory GetFactoryById(int id)
    {
        return Factories.First(f => f.id == id);
    }

    public Utils.DC GetDcById(int id)
    {
        return DCs.First(f => f.id == id);
    }

    public Utils.Team GetTeamById(int id)
    {
        return Teams.First(t => t.id == id);
    }

    public List<Utils.WeekDemand> GetCurrentWeekDemands(int gameinCustomerId)
    {
        return CurrentWeekDemands.Where(d => d.gameinCustomerId == gameinCustomerId) as List<Utils.WeekDemand>;
    }

    public string GetTeamName(int teamId)
    {
        foreach (Utils.Team team in Teams)
        {
            if (teamId == team.id)
            {
                return team.teamName;
            }
        }
        return "Team";
    }
    
    public string GetSupplierName(int supplierId)
    {
        foreach (Utils.Supplier supplier in GameinSuppliers)
        {
            if (supplierId == supplier.id)
            {
                return supplier.name;
            }
        }
        return "Supplier";
    }

    public Utils.Supplier GetSupplierById(int supplierId)
    {
        return GameinSuppliers.FirstOrDefault(s => s.id == supplierId);
    }

    public Utils.GameinCustomer GetCustomerById(int id)
    {
        return GameinCustomers.FirstOrDefault(c => c.id == id);
    }
    
    public Utils.GameinCustomer GetGameinCustomerById(int customerId)
    {
        return GameinCustomers.FirstOrDefault(s => s.id == customerId);
    }
    
    public string GetProductName(int productId)
    {
        foreach (Utils.Product product in Products)
        {
            if (productId == product.id)
            {
                return product.name;
            }
        }
        return "Product";
    }

    public List<Utils.Product> GetRawProducts()
    {
        return Products.Where(p => p.productType == Utils.ProductType.RawMaterial).ToList();
    }

    public List<Utils.Product> GetFinishedProducts()
    {
        return Products.Where(p => p.productType == Utils.ProductType.Finished).ToList();
    }

    public List<Utils.WeekSupply> GetCurrentWeekRawProductSupplies(int rawProductId)
    {
        return CurrentWeekSupplies.Where(s => s.productId == rawProductId).ToList();
    }

    public List<Utils.WeekDemand> GetCurrentWeekFinishedProductDemands(int rawProductId)
    {
        return CurrentWeekDemands.Where(d => d.productId == rawProductId).ToList();
    }

    public Vector2 GetMyTeamLocaionOnMap()
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        Utils.Factory factory = GetFactoryById(GetTeamById(teamId).factoryId);
        return new Vector2((float)factory.latitude, (float)factory.longitude);
    }

    public Vector2 GetLocationByTypeAndId(Utils.TransportNodeType transportNodeType, int transportNodeId)
    {
        switch (transportNodeType)
        {
            case Utils.TransportNodeType.SUPPLIER:
                Utils.Supplier supplier = GameinSuppliers.First(s => s.id == transportNodeId);
                return new Vector2((float)supplier.latitude, (float)supplier.longitude);
            case Utils.TransportNodeType.GAMEIN_CUSTOMER:
                Utils.GameinCustomer gameinCustomer = GameinCustomers.First(c => c.id == transportNodeId);
                return new Vector2((float)gameinCustomer.latitude, (float)gameinCustomer.longitude);
            case Utils.TransportNodeType.DC:
                Utils.DC dc = DCs.First(d => d.id == transportNodeId);
                return new Vector2((float)dc.latitude, (float)dc.longitude);
            case Utils.TransportNodeType.FACTORY:
                Utils.Factory factory = Factories.First(f => f.id == transportNodeId);
                return new Vector2((float)factory.latitude, (float)factory.longitude);
        }

        return Vector2.zero;
    }

    public Utils.Product GetProductById(int id)
    {
        return Products.First(p => p.id == id);
    }

    public Utils.Vehicle GetVehicleByType(Utils.VehicleType vehicleType)
    {
        return Vehicles.First(v => v.vehicleType == vehicleType);
    }

    public bool IsAuctionOver()
    {
        return DateTime.Now > GameConstants.AuctionRoundsStartTime[GameConstants.AuctionRoundsStartTime.Count - 1].ToDateTime().AddSeconds(GameConstants.AuctionRoundDurationSeconds);
    }

    public Utils.ProductionLineTemplate GetProductionLineTemplateById(int templateId)
    {
        return GameDataManager.Instance.ProductionLineTemplates.FirstOrDefault(c => c.id == templateId);
    }
}
