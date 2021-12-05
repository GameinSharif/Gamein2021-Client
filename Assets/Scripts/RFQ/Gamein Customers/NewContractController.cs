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

    private bool _firstTimeInitializing = true;
    private Utils.WeekDemand _weekDemand;
    private List<Utils.Storage> _storages = new List<Utils.Storage>();

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
        if (newContractResponse.contract != null)
        {
            ContractsManager.Instance.AddContractItemToList(newContractResponse.contract);
            NewContractPopupCanvasGameObject.SetActive(false);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenMakeADealPopupClick(Utils.WeekDemand weekDemand)
    {
        _weekDemand = weekDemand;
        
        //TODO clear inputfields
        
        Utils.Product product = GameDataManager.Instance.GetProductById(weekDemand.productId);
        productNameLocalize.SetKey("product_" + product.name);
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        demandAmount.SetKey("new_contract_demand", _weekDemand.amount.ToString());
        customerName.text = GameDataManager.Instance.GetGameinCustomerById(_weekDemand.gameinCustomerId).name;

        if (_firstTimeInitializing)
        {
            InitializeStorageDropdown();
        }
        _firstTimeInitializing = false;

        NewContractPopupCanvasGameObject.SetActive(true);
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

    private bool IsPriceInRange()
    {
        Utils.Product product = GameDataManager.Instance.GetProductById(_weekDemand.productId);
        int price = int.Parse(this.price.text);
        return price >= product.minPrice && price <= product.maxPrice;
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
        string amountText = amount.text;
        string priceText = price.text;
        int weeks = GetRepetitionWeeks();

        if (weeks < 0 || string.IsNullOrEmpty(amountText) || string.IsNullOrEmpty(priceText))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        if (!IsPriceInRange())
        {
            DialogManager.Instance.ShowErrorDialog("price_not_in_range_error");
            return;
        }

        if (HasContractWithDemanderThisWeek())
        {
            DialogManager.Instance.ShowErrorDialog("has_contract_this_week_error");
            return;

        }

        int amountInt = int.Parse(amount.text);
        int priceInt = int.Parse(priceText);

        NewContractRequest newContract = new NewContractRequest(RequestTypeConstant.NEW_CONTRACT, _weekDemand.gameinCustomerId, _storages[sourceStorageDropDown.value].id, _weekDemand.productId, amountInt, priceInt, weeks);
        RequestManager.Instance.SendRequest(newContract);
    }
}
