using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using RTLTMPro;
using UnityEngine.UI;

public class NewContractController : MonoBehaviour
{
    public static NewContractController Instance;

    public GameObject NewContractPopupCanvasGameObject;

    public Image productImage;
    public Localize productNameLocalize;
    public RTLTextMeshPro customerName;
    public TMP_InputField amount;
    public TMP_InputField numberOfRepetition;
    public TMP_InputField price;
    public TMP_Dropdown sourceStorageDropDown;

    public Localize demandAmount;
    public Localize minMaxPriceLocalize;

    private Utils.WeekDemand _weekDemand;
    private List<Utils.Storage> _storages = new List<Utils.Storage>();

    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewContractResponseEvent += OnNewContractResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewContractResponseEvent -= OnNewContractResponse;
    }

    private void OnNewContractResponse(NewContractResponse newContractResponse)
    {
        _isSendingRequest = false;

        if (newContractResponse.contract != null)
        {
            ContractsManager.Instance.AddContractItemToList(newContractResponse.contract);
            
            string productName = GameDataManager.Instance.GetProductName(newContractResponse.contract.productId);
            string translatedProductName =
                LocalizationManager.GetLocalizedValue("product_" + productName,
                    LocalizationManager.GetCurrentLanguage());
            string gameinCustomerName = GameDataManager.Instance.GetGameinCustomerById(newContractResponse.contract.gameinCustomerId).name;
            string[] param = {gameinCustomerName, translatedProductName};
            NotificationsController.Instance.AddNewNotification("notification_new_contract", param);
            
            NewContractPopupCanvasGameObject.SetActive(false);
            CustomersController.Instance.ContractsParentGameObject.SetActive(true);
            CustomersController.Instance.CustomersParentGameObject.SetActive(false);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekDemand weekDemand)
    {
        _weekDemand = weekDemand;
        
        ClearInputFields();
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekDemand.productId);
        productNameLocalize.SetKey("product_" + product.name);
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        demandAmount.SetKey("new_contract_demand", _weekDemand.amount.ToString());
        minMaxPriceLocalize.SetKey("min_max_text", product.minPrice.ToString(), product.maxPrice.ToString());
        customerName.text = GameDataManager.Instance.GetGameinCustomerById(_weekDemand.gameinCustomerId).name;

        InitializeStorageDropdown();

        NewContractPopupCanvasGameObject.SetActive(true);
    }

    private void ClearInputFields()
    {
        amount.text = "";
        numberOfRepetition.text = "";
        price.text = "";
    }

    private void InitializeStorageDropdown()
    {
        sourceStorageDropDown.options.Clear();
        _storages.Clear();

        foreach (Utils.Storage storage in StorageManager.Instance.Storages)
        {
            var optionData = new TMP_Dropdown.OptionData(storage.dc ? "DC " + storage.buildingId : "Warehouse");

            sourceStorageDropDown.options.Add(optionData);
            _storages.Add(storage);
        }

        sourceStorageDropDown.value = CustomersController.Instance.StorageIndex;
    }

    private bool IsPriceInRange(float priceFloat)
    {
        Utils.Product product = GameDataManager.Instance.GetProductById(_weekDemand.productId);
        return priceFloat >= product.minPrice && priceFloat <= product.maxPrice;
    }

    private bool HasContractWithDemanderThisWeek()
    {
        DateTime currentDate = MainHeaderManager.Instance.gameDate.ToDateTime();

        int num_days = DayOfWeek.Friday - currentDate.DayOfWeek;
        if (num_days < 0) num_days += 7;

        DateTime friday = currentDate.AddDays(num_days);

        foreach (Utils.Contract contract in ContractsManager.Instance.myContracts)
        {
            if (contract.gameinCustomerId == _weekDemand.gameinCustomerId && contract.contractDate.ToDateTime() == friday)
            {
                return true;
            }
        }

        return false;
    }
    
    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }
        
        string amountText = amount.text;
        string priceText = price.text;
        string weeksText = numberOfRepetition.text;

        if (string.IsNullOrEmpty(weeksText) || string.IsNullOrEmpty(amountText) || string.IsNullOrEmpty(priceText))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }
        
        int amountInt = int.Parse(amountText);
        int weeksInt = int.Parse(weeksText);
        float priceFloat = float.Parse(priceText);

        if (amountInt < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_amount_error");
            return;
        }

        if (weeksInt < 1 || weeksInt > 10)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_weeks_error");
            return;
        }

        if (!IsPriceInRange(priceFloat))
        {
            DialogManager.Instance.ShowErrorDialog("price_not_in_range_error");
            return;
        }

        if (sourceStorageDropDown.value < 0)
        {
            DialogManager.Instance.ShowErrorDialog("no_storage_selected_error");
            return;
        }

        if (HasContractWithDemanderThisWeek())
        {
            DialogManager.Instance.ShowErrorDialog("has_contract_this_week_error");
            return;

        }

        NewContractRequest newContract = new NewContractRequest(RequestTypeConstant.NEW_CONTRACT, _weekDemand.gameinCustomerId, _storages[sourceStorageDropDown.value].id, _weekDemand.productId, amountInt, priceFloat, weeksInt);
        RequestManager.Instance.SendRequest(newContract);
        _isSendingRequest = true;
    }
}
