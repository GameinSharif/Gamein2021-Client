﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using RTLTMPro;
using UnityEngine.UI;

public class MakeADealWithSupplierPopupController : MonoBehaviour
{
    public static MakeADealWithSupplierPopupController Instance;

    public GameObject makeADealWithSupplierPopupCanvas;

    public Image productImage;
    public Localize productNameLocalize;
    public TMP_InputField amount;
    public RTLTextMeshPro pricePerUnit;
    public TMP_InputField totalPrice;
    public TMP_InputField finalPrice;
    public TMP_InputField arrivalDate;
    public ToggleGroup transportationMode;
    public TMP_InputField numberOfRepetition;
    public TMP_InputField penalty; //TODO calculate?
    public ToggleGroup wantsInsurance;

    private Utils.WeekSupply _weekSupply;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewContractSupplierResponseEvent += OnNewContractSupplierResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewContractSupplierResponseEvent -= OnNewContractSupplierResponse;
    }

    private void OnNewContractSupplierResponse(NewContractSupplierResponse newContractSupplierResponse)
    {
        if (newContractSupplierResponse.result == "Successful")
        {
            GameinSuppliersController.Instance.AddContractItemsToList(newContractSupplierResponse.contractSuppliers);
            makeADealWithSupplierPopupCanvas.SetActive(false);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekSupply weekSupply)
    {
        _weekSupply = weekSupply;
        
        //TODO clear inputfields
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekSupply.productId);
        productNameLocalize.SetKey("product_" + product.name);
        pricePerUnit.text = weekSupply.price + "$";
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        CustomDate date = MainHeaderManager.Instance.gameDate.AddDays(GetTransportDuration());
        arrivalDate.text = date.ToString();
        makeADealWithSupplierPopupCanvas.SetActive(true);
    }

    private int GetTransportDuration()
    {
        Utils.VehicleType vehicleType = GetTransportationMode();
        Vector2 destinationLocation = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        Vector2 sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.SUPPLIER, _weekSupply.supplierId);
        int duration = TransportManager.Instance.CalculateTransportDuration(sourceLocation, destinationLocation, vehicleType);
        return duration;
    }
    
    public void OnAmountValueChange()
    {
        string amount = this.amount.text;
        if (string.IsNullOrEmpty(amount))
        {
            return;
        }

        int total = int.Parse(amount) * _weekSupply.price;
        float transportationCost = GetTransportCost(int.Parse(amount));
        float final = total + transportationCost;
        totalPrice.text = total.ToString("0.00") + "$";
        finalPrice.text = final.ToString("0.00") + "$";
    }

    private float GetTransportCost(int amount)
    {
        Utils.VehicleType vehicleType = GetTransportationMode();
        Vector2 destinationLocation = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        Vector2 sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.SUPPLIER, _weekSupply.supplierId);
        int distance = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation, vehicleType);
        float cost = TransportManager.Instance.CalculateTransportCost(vehicleType, distance, _weekSupply.productId, amount, WantsInsurance());
        return cost;
    }

    private bool WantsInsurance()
    {
        Toggle wants = wantsInsurance.ActiveToggles().FirstOrDefault();
        if (wants.name == "Yes")
        {
            return true;
        }

        return false;
    }

    private Utils.VehicleType GetTransportationMode()
    {
        Toggle mode = transportationMode.ActiveToggles().FirstOrDefault();
        switch (mode.name)
        {
            case "AirPlane":
                return Utils.VehicleType.AIRPLANE;
            case "Train":
                return Utils.VehicleType.TRAIN;
            case "Truck":
                return Utils.VehicleType.TRUCK;
            case "Vanet":
                return Utils.VehicleType.VANET;
            default:
                return Utils.VehicleType.AIRPLANE;
        }
    }

    public void OnTransportationModeToggleChange()
    {
        OnAmountValueChange();
        CustomDate date = MainHeaderManager.Instance.gameDate.AddDays(GetTransportDuration());
        arrivalDate.text = date.ToString();
    }

    public void OnInsuranceToggleChange()
    {
        OnAmountValueChange();
    }
    
    private int GetRepetitionWeeks()
    {
        string weeks = numberOfRepetition.text;
        if (string.IsNullOrEmpty(weeks))
        {
            return 0;
        }
        int weeksInt = int.Parse(numberOfRepetition.text);
        if (weeksInt < 0)
        {
            return -1;
        }
        return weeksInt;
    }

    private bool CanAffordMakingContract()
    {
        float currentMoney = MainHeaderManager.Instance.Money;
        int total = int.Parse(amount.text) * _weekSupply.price;
        float transportationCost = GetTransportCost(int.Parse(amount.text));
        float final = total + transportationCost;

        return final <= currentMoney;
    }
    
    public void OnDoneButtonClick()
    {
        Debug.LogWarning(1);
        string amountText = amount.text;
        Utils.VehicleType vehicleType = GetTransportationMode();
        int weeks = GetRepetitionWeeks();
        if (weeks < 0 || string.IsNullOrEmpty(amountText))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }
        Debug.LogWarning(2);
        Debug.LogWarning(vehicleType);
        if (CanAffordMakingContract())
        {
            int amountInt = int.Parse(amount.text);
            NewContractSupplierRequest newContractSupplier = new NewContractSupplierRequest(RequestTypeConstant.NEW_CONTRACT_WITH_SUPPLIER, _weekSupply, weeks, amountInt, GameDataManager.Instance.GetVehicleByType(vehicleType).id);
            RequestManager.Instance.SendRequest(newContractSupplier);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
        }
    }
}
