using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using RTLTMPro;
using UnityEngine.UI;

public class MakeADealWithDemanderPopupController : MonoBehaviour
{
    public static MakeADealWithDemanderPopupController Instance;

    public GameObject makeADealWithDemanderPopupCanvas;

    public Image productImage;
    public Localize productNameLocalize;
    public TMP_InputField amount;
    public TMP_InputField numberOfRepetition;
    public TMP_InputField price;

    private Utils.WeekDemand _weekDemand;

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
            List<Utils.Contract> contracts = new List<Utils.Contract>();
            contracts.Add(newContractResponse.contract);
            ContractsManager.Instance.AddContractItemsToList(contracts);
            //Utils.Contract firstContract = contracts[0];
            makeADealWithDemanderPopupCanvas.SetActive(false);
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
        makeADealWithDemanderPopupCanvas.SetActive(true);
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
        //TODO how to get date of current contract
        CustomDate thisWeekDate = new CustomDate(2020, 12, 3);
        List<Utils.Contract> contracts = ContractsManager.Instance.myContracts;
        bool result = false;
        foreach (Utils.Contract contract in contracts)
        {
            if (contract.gameinCustomerId == _weekDemand.gameinCustomerId && contract.contractDate == thisWeekDate)
            {
                result = true;
            }
        }

        return result;
    }
    
    public void OnDoneButtonClick()
    {
        Debug.LogWarning(1);
        string amountText = amount.text;
        string priceText = price.text;
        int weeks = GetRepetitionWeeks();
        if (weeks < 0 || string.IsNullOrEmpty(amountText) || string.IsNullOrEmpty(priceText))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }
        Debug.LogWarning(2);
        if (IsPriceInRange() && !HasContractWithDemanderThisWeek())
        {
            int amountInt = int.Parse(amount.text);
            int priceInt = int.Parse(priceText);
            NewContractRequest newContract = new NewContractRequest(RequestTypeConstant.NEW_CONTRACT, _weekDemand.gameinCustomerId, _weekDemand.productId, amountInt,
                priceInt, weeks);
            RequestManager.Instance.SendRequest(newContract);
        }
        if (!IsPriceInRange())
        {
            DialogManager.Instance.ShowErrorDialog("price_not_in_range_error");
        }

        if (HasContractWithDemanderThisWeek())
        {
            DialogManager.Instance.ShowErrorDialog("has_contract_this_week_error");

        }
    }
}
