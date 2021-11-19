using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class SetContractDetail : MonoBehaviour
{
    [HideInInspector] public Utils.Contract contractData;

    public RTLTextMeshPro rowNumberTxt;
    public RTLTextMeshPro gameinCustomerNameTxt;
    public RTLTextMeshPro productNameTxt;
    public RTLTextMeshPro contractDateTxt;
    public RTLTextMeshPro supplyAmountTxt;
    public RTLTextMeshPro pricePerUnitTxt;
    public RTLTextMeshPro boughtAmountTxt;
    public RTLTextMeshPro currentWeekTeamBrandTxt;
    public RTLTextMeshPro sharePercentageTxt;
    public RTLTextMeshPro incomePercentageTxt;
    public RTLTextMeshPro currentWeekPriceRangeTxt;
    public RTLTextMeshPro terminatePenaltyTxt;
    public RTLTextMeshPro lostSalePenaltyTxt;
    
    public GameObject terminateButtonGameObject;

    public void InitializeContract(Utils.Contract contractData, int index)
    {
        rowNumberTxt.text = (index + 1).ToString();
        gameinCustomerNameTxt.text = GameDataManager.Instance.GetGameinCustomerById(contractData.gameinCustomerId).name;
        productNameTxt.text = GameDataManager.Instance.Products[contractData.productId].name;
        contractDateTxt.text = contractData.contractDate.ToString();
        supplyAmountTxt.text = contractData.supplyAmount.ToString();
        pricePerUnitTxt.text = contractData.pricePerUnit.ToString();
        boughtAmountTxt.text = contractData.boughtAmount.ToString();
        //TODO how to get brand, share, income, price range
        currentWeekTeamBrandTxt.text = "brand";
        sharePercentageTxt.text = "share%";
        incomePercentageTxt.text = "income%";
        currentWeekPriceRangeTxt.text = "range";
        terminatePenaltyTxt.text = contractData.terminatePenalty.ToString();
        lostSalePenaltyTxt.text = contractData.lostSalePenalty.ToString();

        this.contractData = contractData;
    }

    public void OnShowDetailsButtonClick()
    {
        //TODO
    }

    public void OnTerminateContractButtonClick()
    {
        //TODO
    }
}
