using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RTLTMPro;
using UnityEngine.UI;

public class MakeADealWithSupplierPopupController : MonoBehaviour
{
    public static MakeADealWithSupplierPopupController Instance;

    public GameObject makeADealWithSupplierPopupCanvas;
    public GameObject longtermModeParent;

    public Image productImage;
    public Localize productNameLocalize;
    public TMP_InputField amount;
    public RTLTextMeshPro pricePerUnit;
    public TMP_InputField totalPrice;
    public TMP_InputField finalPrice;
    public TMP_InputField arrivalDate; //TODO calculate it and then show it
    public ToggleGroup repetition;
    public ToggleGroup transportationMode;
    public TMP_InputField numberOfRepetition;
    public TMP_InputField penalty; //TODO calculate it and then show it

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
            GameinSuppliersController.Instance.AddContractItemToList(newContractSupplierResponse.contractSupplier);

            makeADealWithSupplierPopupCanvas.SetActive(false);
        }
        else
        {
            //TODO show error
        }
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekSupply weekSupply)
    {
        this._weekSupply = weekSupply;
        
        //TODO clear inputfields
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekSupply.productId);
        productNameLocalize.SetKey("product_" + product.name);
        pricePerUnit.text = weekSupply.price + "$";
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        
        makeADealWithSupplierPopupCanvas.SetActive(true);
    }
    
    public void OnAmountValueChange()
    {
        string amount = this.amount.text;
        if (string.IsNullOrEmpty(amount))
        {
            return;
        }

        int total = int.Parse(amount) * _weekSupply.price;
        //TODO calculate transportation cost
        int transportationCost = 100;
        int final = total + transportationCost;
        totalPrice.text = total.ToString("0.00") + "$";
        finalPrice.text = final.ToString("0.00") + "$";
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
        longtermModeParent.SetActive(mode.name != "Once");
    }

    private int GetRepetitionWeeks()
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
        Debug.LogWarning(1);
        string amountText = amount.text;
        Utils.VehicleType vehicleType = GetTransportationMode();
        int weeks = GetRepetitionWeeks();
        if (weeks < 0 || string.IsNullOrEmpty(amountText))
        {
            //TODO show error
            return;
        }
        Debug.LogWarning(2);
        Debug.LogWarning(vehicleType);
        int amountInt = int.Parse(this.amount.text);
        NewContractSupplierRequest newContractSupplier = new NewContractSupplierRequest(RequestTypeConstant.NEW_CONTRACT_WITH_SUPPLIER, _weekSupply, weeks, amountInt, GameDataManager.Instance.GetVehicleByType(vehicleType).id);
        RequestManager.Instance.SendRequest(newContractSupplier);
    }
}
