using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageTransportPopupController : MonoBehaviour
{
    public static StorageTransportPopupController Instance;

    public GameObject popup;

    public TMP_Dropdown destinationDropDown;
    public TMP_Dropdown vehicleTypeDropDown;
    public Localize sourceNameLocalize;
    public TMP_InputField amountInputField;
    public Toggle insurance;
    public TMP_InputField totalCost;
    public ProductDetailsSetter productDetailsSetter;

    private Utils.Storage _source;
    private Utils.Product _product;
    private bool _firstTimeInitializing = true;
    private float _totalCostValue;

    private List<Utils.VehicleType> _vehicleTypesOptions = new List<Utils.VehicleType>();
    private List<Utils.Storage> _destinationOptions = new List<Utils.Storage>();

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnStartTransportForPlayerStoragesResponseEvent += OnStartTransportForPlayerStoragesResponse;
    }

    public void Initialize(Utils.Storage source, Utils.Product product)
    {
        _source = source;
        _product = product;

        StorageManager.SetStorageLocalize(sourceNameLocalize, _source);
        productDetailsSetter.SetRawData(_product);

        if (_firstTimeInitializing)
        {
            InitializeVehicleDropdown();
        }
        _firstTimeInitializing = false;
        
        InitializeDestinationDropdown();

        popup.SetActive(true);
    }

    private void InitializeVehicleDropdown()
    {
        vehicleTypeDropDown.options.Clear();
        _vehicleTypesOptions.Clear();
        foreach (Utils.VehicleType vehicleType in Enum.GetValues(typeof(Utils.VehicleType)))
        {
            var optionData = new TMP_Dropdown.OptionData(vehicleType.ToString());
            vehicleTypeDropDown.options.Add(optionData);
            _vehicleTypesOptions.Add(vehicleType);
        }
        
        vehicleTypeDropDown.value = 0;
    }

    private void InitializeDestinationDropdown()
    {
        destinationDropDown.options.Clear();
        _destinationOptions.Clear();
        foreach (var storage in StorageManager.Instance.Storages)
        {
            if (storage.id == _source.id) continue;
            
            var optionData = new TMP_Dropdown.OptionData(storage.dc ? "DC " : "Warehouse " + storage.buildingId);
            destinationDropDown.options.Add(optionData);
            _destinationOptions.Add(storage);
        }

        destinationDropDown.value = 0;
    }

    public void OnSendButtonClicked()
    {
        //valid input check
        
        var amount = ParseAmount();
        if (amount == null || amount < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_amount_error");
            return;
        }
        
        var sourceType = _source.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        var destination = _destinationOptions[destinationDropDown.value];
        var destinationType = destination.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        var vehicleType = _vehicleTypesOptions[vehicleTypeDropDown.value];

        // dc type check
        
        if (destination.dc)
        {
            var dc = GameDataManager.Instance.GetDcById(destination.buildingId);
            bool typeMatches =
                (dc.type == Utils.DCType.SemiFinished && _product.productType == Utils.ProductType.SemiFinished) ||
                (dc.type == Utils.DCType.Finished && _product.productType == Utils.ProductType.Finished);
            if (!typeMatches)
            {
                DialogManager.Instance.ShowErrorDialog("dc_and_product_type_mismatch_error");
                return; 
            }
        }

        //available amount check
        
        var availableAmount = StorageManager.Instance.GetProductAmountByStorage(_source, _product.id);
        if (availableAmount < amount)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_available_amount_error");
            return;
        }

        //enough available capacity check
        
        var availableCapacity = StorageManager.Instance.CalculateAvailableCapacity(destination, _product.productType, true);
        if (availableCapacity < amount * _product.volumetricUnit)
        {
            DialogManager.Instance.ShowErrorDialog("destination_capacity_error");
            return; 
        }
        
        //money check

        if (MainHeaderManager.Instance.Money < _totalCostValue)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
            return;
        }


        var request = new StartTransportForPlayerStoragesRequest(
            sourceId: _source.id,
            sourceType: sourceType,
            destinationId: destination.id,
            destinationType: destinationType,
            productId: _product.id,
            amount: amount.Value,
            hasInsurance: insurance.isOn,
            vehicleType: vehicleType
        );
        RequestManager.Instance.SendRequest(request);
    }

    public void RefreshTotalCost()
    {

        var amount = ParseAmount();
        if (amount == null || amount < 1) return;
        
        var destination = _destinationOptions[destinationDropDown.value];
        var destinationType = destination.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        var vehicleType = _vehicleTypesOptions[vehicleTypeDropDown.value];

        var sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(
            _source.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY, _source.buildingId);
        
        var destinationLocation = GameDataManager.Instance.GetLocationByTypeAndId(destinationType, destination.buildingId);
        
        int distance = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation, vehicleType);

        _totalCostValue =
            TransportManager.Instance.CalculateTransportCost(vehicleType, distance, _product.id, amount.Value,
                insurance.isOn);
        totalCost.text = _totalCostValue.ToString();
    }

    private int? ParseAmount()
    {
        int? amount = null;
        try
        {
            amount = int.Parse(amountInputField.text);
        }
        catch (Exception e)
        { }

        return amount;
    }

    private void OnStartTransportForPlayerStoragesResponse(StartTransportForPlayerStoragesResponse response)
    {
        Debug.Log("start transport response: " + response.response);
    }
}