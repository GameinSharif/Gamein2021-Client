using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageTransportPopupController : MonoBehaviour
{
    public static StorageTransportPopupController Instance;

    public GameObject popup;

    public Localize title;
    public TMP_Dropdown destinationDropDown;
    public TMP_Dropdown vehicleTypeDropDown;
    public TMP_InputField amountInputField;
    public Toggle insurance;
    public TMP_InputField totalCost;

    private Utils.Storage _source;
    private Utils.Product _product;
    private bool _firstTimeInitializing = true;
    private float _totalCostValue;

    private bool _isSendingRequest = false;

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
        
        //TODO find a way for title to be of format "Transfer {_product.name}"
        title.SetKey("product_" + _product.name);

        if (_firstTimeInitializing)
        {
            InitializeVehicleDropdown();
            _firstTimeInitializing = false;
        }

        InitializeDestinationDropdown();

        ClearInputFields();
        
        popup.SetActive(true);
    }

    private void ClearInputFields()
    {
        destinationDropDown.value = -1;
        vehicleTypeDropDown.value = -1;
        amountInputField.text = "";
        insurance.SetIsOnWithoutNotify(false);
        totalCost.text = "";
    }

    private void InitializeVehicleDropdown()
    {
        vehicleTypeDropDown.options.Clear();
        _vehicleTypesOptions.Clear();
        foreach (Utils.VehicleType vehicleType in Enum.GetValues(typeof(Utils.VehicleType)))
        {
            TempLocalization.Instance.localize.SetKey(vehicleType.ToString());
            var optionData = new TMP_Dropdown.OptionData(TempLocalization.Instance.localize.GetLocalizedString().value);
            vehicleTypeDropDown.options.Add(optionData);
            _vehicleTypesOptions.Add(vehicleType);
        }
        
        vehicleTypeDropDown.value = -1;
    }

    private void InitializeDestinationDropdown()
    {
        destinationDropDown.options.Clear();
        _destinationOptions.Clear();
        foreach (var storage in StorageManager.Instance.Storages)
        {
            if (storage.id == _source.id) continue;
            
            TempLocalization.Instance.localize.SetKey(storage.dc ? "provider_item_dc" : "provider_item_warehouse");
            string value = TempLocalization.Instance.localize.GetLocalizedString().value;

            if (storage.dc)
            {
                int index = value.IndexOf('#');
                value = value.Remove(index, 1).Insert(index, storage.buildingId.ToString());
            }

            var optionData = new TMP_Dropdown.OptionData(value);
            destinationDropDown.options.Add(optionData);
            _destinationOptions.Add(storage);
        }

        destinationDropDown.value = -1;
    }

    public void OnSendButtonClicked()
    {
        if (_isSendingRequest)
        {
            return;
        }
        
        //valid input check
        
        var amount = ParseAmount();
        if (amount == null || amount < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_amount_error");
            return;
        }

        if (destinationDropDown.value < 0)
        {
            DialogManager.Instance.ShowErrorDialog("destination_not_selected_error");
            return;
        }

        if (vehicleTypeDropDown.value < 0)
        {
            DialogManager.Instance.ShowErrorDialog("vehicle_not_selected_error");
            return;
        }
        
        Utils.TransportNodeType sourceType = _source.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        Utils.Storage destination = _destinationOptions[destinationDropDown.value];
        Utils.TransportNodeType destinationType = destination.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        Utils.VehicleType vehicleType = _vehicleTypesOptions[vehicleTypeDropDown.value];

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
            sourceId: _source.buildingId,
            sourceType: sourceType,
            destinationId: destination.buildingId,
            destinationType: destinationType,
            productId: _product.id,
            amount: amount.Value,
            hasInsurance: insurance.isOn,
            vehicleType: vehicleType
        );
        RequestManager.Instance.SendRequest(request);
        _isSendingRequest = true;
    }

    public void RefreshTotalCost()
    {
        var amount = ParseAmount();
        if (amount == null || amount < 1 || destinationDropDown.value < 0 || vehicleTypeDropDown.value < 0)
        {
            totalCost.text = "";
            return;
        }
        
        var destination = _destinationOptions[destinationDropDown.value];
        var destinationType = destination.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY;
        var vehicleType = _vehicleTypesOptions[vehicleTypeDropDown.value];

        var sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(
            _source.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY, _source.buildingId);
        
        var destinationLocation = GameDataManager.Instance.GetLocationByTypeAndId(destinationType, destination.buildingId);
        
        int distance = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation, vehicleType);

        _totalCostValue = TransportManager.Instance.CalculateTransportCost(vehicleType, distance, _product.id, amount.Value, insurance.isOn);
        totalCost.text = _totalCostValue.ToString("0.00");
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
        _isSendingRequest = false;
        Debug.Log("start transport response: " + response.response);

        if (response.transportDto == null)
        {
            DialogManager.Instance.ShowErrorDialog();
        }
        else
        {
            popup.SetActive(false);
        }
    }
}