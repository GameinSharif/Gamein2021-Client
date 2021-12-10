using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class SetContractDetail : MonoBehaviour
{
    [HideInInspector] public Utils.Contract contractData;

    public RTLTextMeshPro gameinCustomerNameTxt;
    public Localize productNameLocalize;
    public RTLTextMeshPro contractDateTxt;
    public RTLTextMeshPro supplyAmountTxt;
    public RTLTextMeshPro pricePerUnitTxt;
    public RTLTextMeshPro boughtAmountTxt;
    public RTLTextMeshPro demandShareTxt;
    
    public GameObject terminateButtonGameObject;

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateLongtermContractResponseEvent += OnTerminateLongtermContractResponseReceived;
        EventManager.Instance.OnContractFinalizedResponseEvent += OnContractFinalizedRespinseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateLongtermContractResponseEvent -= OnTerminateLongtermContractResponseReceived;
        EventManager.Instance.OnContractFinalizedResponseEvent -= OnContractFinalizedRespinseReceived;
    }
    
    public void InitializeContract(Utils.Contract contractData)
    {
        gameinCustomerNameTxt.text = GameDataManager.Instance.GetGameinCustomerById(contractData.gameinCustomerId).name;
        productNameLocalize.SetKey("product_" + GameDataManager.Instance.Products[contractData.productId].name);
        contractDateTxt.text = contractData.contractDate.ToString();
        supplyAmountTxt.text = contractData.supplyAmount.ToString();
        pricePerUnitTxt.text = contractData.pricePerUnit.ToString("0.00");
        boughtAmountTxt.text = contractData.boughtAmount.ToString();
        demandShareTxt.text = contractData.demandShare.ToString("0.00") + "%";

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
                gameObject.SetActive(false);
            }
        }
    }

    public void OnContractFinalizedRespinseReceived(ContractFinalizedResponse contractFinalizedResponse)
    {
        if (contractData.id == contractFinalizedResponse.contract.id)
        {
            InitializeContract(contractFinalizedResponse.contract);
            if (ContractMoreDetailsController.contractData != null && ContractMoreDetailsController.contractData.id == contractFinalizedResponse.contract.id)
            {
                ContractMoreDetailsController.Instance.ShowMoreDetailsButtonClick(contractFinalizedResponse.contract);
            }

            //TODO notification
        }
    }

    public void OnShowMoreDetailsButtonClick()
    {
        ContractMoreDetailsController.Instance.ShowMoreDetailsButtonClick(contractData);
    }
}
