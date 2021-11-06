using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RTLTMPro;
using UnityEngine.UI;

public class MakeAdealWithSupplierPopupController : MonoBehaviour
{
    public static MakeAdealWithSupplierPopupController Instance;

    public GameObject MakeAdealWithSupplierPopupCanvas;
    public GameObject LongtermModeParent;

    public Image ProductImage;
    public Localize ProductNameLocalize;
    public TMP_InputField amount;
    public RTLTextMeshPro PricePerUnit;
    public RTLTextMeshPro TotalPrice;
    public RTLTextMeshPro FinalPrice;
    public DatePicker arrivalDate;
    public ToggleGroup repetition;
    public ToggleGroup transportationMode;
    public TMP_InputField numberOfRepetition;
    public RTLTextMeshPro penalty;

    public Utils.WeekSupply weekSupply;

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
            GameinSuppliersController.Instance.AddContractItemToList(newContractSupplierResponse.contractSupplier);

            MakeAdealWithSupplierPopupCanvas.SetActive(false);
        }
        else
        {
            //TODO show error
        }
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekSupply weekSupply)
    {
        this.weekSupply = weekSupply;
        
        //TODO clear inputfields
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekSupply.productId);
        ProductNameLocalize.SetKey("product_" + product.name);
        PricePerUnit.text = weekSupply.price + "$";
        ProductImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        
        MakeAdealWithSupplierPopupCanvas.SetActive(true);
    }
    
    public void OnAmountValueChange()
    {
        string amount = this.amount.text;
        if (string.IsNullOrEmpty(amount))
        {
            return;
        }

        int total = int.Parse(amount) * weekSupply.price;
        //TODO calculate transportation cost
        int transportationCost = 100;
        int final = total + transportationCost;
        TotalPrice.text = total.ToString("0.00") + "$";
        FinalPrice.text = final.ToString("0.00") + "$";
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

    public void OnRepetitionToggleChange()
    {
        Toggle mode = repetition.ActiveToggles().FirstOrDefault();
        if (mode.name == "Once")
        {
            LongtermModeParent.SetActive(false);
        }
        else
        {
            LongtermModeParent.SetActive(true);
            //TODO clear input fields
        }
    }

    public int GetRepetitionWeeks()
    {
        Toggle mode = repetition.ActiveToggles().FirstOrDefault();
        if (mode.name == "Once")
        {
            return 0;
        }
        string weeks = numberOfRepetition.text;
        if (string.IsNullOrEmpty(weeks))
        {
            return -1;
        }
        return int.Parse(numberOfRepetition.text);
    }
    
    public void OnDoneButtonClick()
    {
        string amountText = amount.text;
        Utils.VehicleType vehicleType = GetTransportationMode();
        int weeks = GetRepetitionWeeks();
        if (weeks < 0 || string.IsNullOrEmpty(amountText))
        {
            //TODO show error
            return;
        }
        int amountInt = int.Parse(this.amount.text);
        NewContractSupplierRequest newContractSupplier = new NewContractSupplierRequest(RequestTypeConstant.NEW_CONTRACT_SUPPLIER, weekSupply, weeks, amountInt, vehicleType);
        RequestManager.Instance.SendRequest(newContractSupplier);
    }
}
