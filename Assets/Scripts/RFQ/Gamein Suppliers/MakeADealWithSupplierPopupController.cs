using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using RTLTMPro;
using UnityEngine.UI;

public class MakeADealWithSupplierPopupController : MonoBehaviour
{
    public static MakeADealWithSupplierPopupController Instance;

    public GameObject makeADealWithSupplierPopupCanvas;

    public Image productImage;
    public Localize productNameLocalize;
    public RTLTextMeshPro supplierName;

    public TMP_InputField amount;
    public TMP_Dropdown vehicleTypeDropDown;
    public TMP_InputField numberOfWeeks;
    public Toggle insurance;
    public TMP_InputField totalPrice;
    public TMP_InputField arrivalDate;

    private bool _firstTimeInitializing = true;
    private List<Utils.VehicleType> _vehicleTypesOptions = new List<Utils.VehicleType>();
    private Utils.WeekSupply _weekSupply;

    private bool _isSendingRequest = false;

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
        _isSendingRequest = false;

        if (newContractSupplierResponse.result == "Successful")
        {
            List<Utils.ContractSupplier> contractSuppliers = newContractSupplierResponse.contractSuppliers;
            GameinSuppliersController.Instance.AddContractItemsToList(contractSuppliers);

            Utils.ContractSupplier firstContract = contractSuppliers[0];
            float cost = firstContract.boughtAmount * firstContract.pricePerUnit + firstContract.transportationCost;
            MainHeaderManager.Instance.Money -= cost;
            
            string productName = GameDataManager.Instance.GetProductName(newContractSupplierResponse.contractSuppliers[0].materialId);
            string translatedProductName =
                LocalizationManager.GetLocalizedValue("product_" + productName,
                    LocalizationManager.GetCurrentLanguage());
            string gameinSupplierName = GameDataManager.Instance.GetSupplierName(newContractSupplierResponse.contractSuppliers[0].supplierId);
            string[] param = {gameinSupplierName, translatedProductName};
            NotificationsController.Instance.AddNewNotification("notification_new_contract_supplier", param);

            makeADealWithSupplierPopupCanvas.SetActive(false);
            SuppliersController.Instance.ContractsParentGameObject.SetActive(false);
            SuppliersController.Instance.SuppliersParentGameObject.SetActive(true);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
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
        
        vehicleTypeDropDown.value = 0;
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekSupply weekSupply)
    {
        _weekSupply = weekSupply;
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekSupply.productId);
        productNameLocalize.SetKey("product_" + product.name);
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        supplierName.text = GameDataManager.Instance.GetSupplierName(weekSupply.supplierId);

        if (_firstTimeInitializing)
        {
            InitializeVehicleDropdown();
        }

        _firstTimeInitializing = false;

        ClearInputFields();

        SetArrivalDate();

        makeADealWithSupplierPopupCanvas.SetActive(true);
    }

    private void ClearInputFields()
    {
        amount.text = "";
        numberOfWeeks.text = "";
        totalPrice.text = "";
        arrivalDate.text = "";
        insurance.SetIsOnWithoutNotify(false);
        vehicleTypeDropDown.value = 0;
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
            totalPrice.text = "";
            return;
        }

        float transportationCost = GetTransportCost(int.Parse(amount));

        float final = int.Parse(amount) * _weekSupply.price + transportationCost;
        totalPrice.text = final.ToString("0.00");
    }

    private float GetTransportCost(int amount)
    {
        Utils.VehicleType vehicleType = GetTransportationMode();
        Vector2 destinationLocation = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        Vector2 sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.SUPPLIER, _weekSupply.supplierId);
        int distance = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation, vehicleType);
        return TransportManager.Instance.CalculateTransportCost(vehicleType, distance, _weekSupply.productId, amount, insurance.isOn);
    }

    private Utils.VehicleType GetTransportationMode()
    {
        var vehicleType = _vehicleTypesOptions[vehicleTypeDropDown.value];
        return vehicleType;
    }

    public void OnTransportationModeToggleChange()
    {
        OnAmountValueChange();
        SetArrivalDate();
    }

    public void OnInsuranceToggleChange()
    {
        OnAmountValueChange();
    }
    
    private int GetNumberOfWeeks()
    {
        string weeks = numberOfWeeks.text;
        if (string.IsNullOrEmpty(weeks))
        {
            return 0;
        }
        int weeksInt = int.Parse(numberOfWeeks.text);
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

    private bool StorageHasCapacity()
    {
        int amount = int.Parse(this.amount.text);
        Utils.Product product = GameDataManager.Instance.GetProductById(_weekSupply.productId);
        int neededCapacity = amount * product.volumetricUnit;
        int teamId = PlayerPrefs.GetInt("TeamId");
        Utils.Factory factory = GameDataManager.Instance.GetFactoryById(GameDataManager.Instance.GetTeamById(teamId).factoryId);
        Utils.Storage storage = StorageManager.Instance.GetStorageByBuildingIdAndType(factory.id, false);
        int capacity = StorageManager.Instance.CalculateAvailableCapacity(storage, Utils.ProductType.RawMaterial, true);
        return neededCapacity <= capacity;
    }
    
    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }

        string amountText = amount.text;
        if (string.IsNullOrEmpty(amountText) || int.Parse(amountText) < 1)
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        int weeks = GetNumberOfWeeks();
        if (weeks < 1 || weeks > 10)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_weeks_error");
            return;
        }

        Utils.VehicleType vehicleType = GetTransportationMode();
        if (vehicleTypeDropDown.value < 0)
        {
            DialogManager.Instance.ShowErrorDialog("vehicle_not_selected_error");
            return;
        }

        if (!CanAffordMakingContract())
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
            return;
        }

        if (!StorageHasCapacity())
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_capacity_error");
            return;
        }

        int amountInt = int.Parse(amount.text);
        NewContractSupplierRequest newContractSupplier = new NewContractSupplierRequest(RequestTypeConstant.NEW_CONTRACT_WITH_SUPPLIER, _weekSupply, weeks, amountInt, GameDataManager.Instance.GetVehicleByType(vehicleType).id, insurance.isOn);
        RequestManager.Instance.SendRequest(newContractSupplier);
        _isSendingRequest = true;
    }

    private void SetArrivalDate()
    {
        DateTime dateTime = MainHeaderManager.Instance.gameDate.ToDateTime().AddDays(GetTransportDuration());
        arrivalDate.text = dateTime.Year + "/" +
            dateTime.Month.ToString().PadLeft(2, '0') + "/" +
            dateTime.Day.ToString().PadLeft(2, '0'); ;
    }
}
