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
    public RTLTextMeshPro demandShareTxt;
    public RTLTextMeshPro valueShareTxt;
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
        pricePerUnitTxt.text = contractData.pricePerUnit.ToString("0.00") + "$";
        boughtAmountTxt.text = contractData.boughtAmount.ToString();
        currentWeekTeamBrandTxt.text = contractData.currentBrand.ToString("0.00");
        demandShareTxt.text = contractData.demandShare.ToString("0.00");
        valueShareTxt.text = contractData.valueShare.ToString("0.00");
        currentWeekPriceRangeTxt.text = contractData.minPrice.ToString("0.00") + " - " + contractData.maxPrice.ToString("0.00");
        terminatePenaltyTxt.text = contractData.terminatePenalty.ToString();
        lostSalePenaltyTxt.text = contractData.lostSalePenalty.ToString();

        this.contractData = contractData;
    }
    
    public void OnTerminateButtonClicked()
    {
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                TerminateLongtermContractRequest terminateLongtermContract = new TerminateLongtermContractRequest(RequestTypeConstant.TERMINATE_CONTRACT, contractData.id);
                RequestManager.Instance.SendRequest(terminateLongtermContract);
            }
        });
    }

    
    public void OnTerminateLongtermContractResponseReceived(TerminateLongtermContractResponse terminateLongtermContractResponse)
    {
        if (terminateLongtermContractResponse.terminatedContract != null)
        {
            if (contractData.id == terminateLongtermContractResponse.terminatedContract.id)
            {
                DialogManager.Instance.ShowErrorDialog("contract_supplier_successfully_terminated");
                MainHeaderManager.Instance.Money -= terminateLongtermContractResponse.terminatedContract.terminatePenalty;
                terminateButtonGameObject.SetActive(false);
            }
        }
    }
}
