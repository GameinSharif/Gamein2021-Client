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
    public Localize storageLocalize;

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
        productNameLocalize.SetKey("product_" + GameDataManager.Instance.GetProductName(contractData.productId));
        supplyAmountTxt.text = contractData.supplyAmount.ToString();
        pricePerUnitTxt.text = contractData.pricePerUnit.ToString("0.00");

        contractDateTxt.text = contractData.contractDate.ToString();
        if (contractData.contractDate.ToDateTime() > MainHeaderManager.Instance.gameDate.ToDateTime())
        {
            boughtAmountTxt.text = "-";
            terminateButtonGameObject.SetActive(true);
        }
        else
        {
            boughtAmountTxt.text = contractData.boughtAmount.ToString();
            terminateButtonGameObject.SetActive(false);
        }

        Utils.Storage storage = StorageManager.Instance.GetStorageById(contractData.storageId);
        if (storage.dc)
        {
            storageLocalize.SetKey("provider_item_dc", storage.buildingId.ToString());
        }
        else
        {
            storageLocalize.SetKey("provider_item_warehouse");
        }

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
        }, "terminate_contract", contractData.terminatePenalty.ToString());
    }
    
    public void OnTerminateLongtermContractResponseReceived(TerminateLongtermContractResponse terminateLongtermContractResponse)
    {
        if (terminateLongtermContractResponse.terminatedContract != null)
        {
            if (contractData.id == terminateLongtermContractResponse.terminatedContract.id)
            {
                string productName = GameDataManager.Instance.GetProductName(terminateLongtermContractResponse.terminatedContract.productId);
                string translatedProductName =
                    LocalizationManager.GetLocalizedValue("product_" + productName,
                        LocalizationManager.GetCurrentLanguage());
                string gameinCustomerName = GameDataManager.Instance.GetGameinCustomerById(terminateLongtermContractResponse.terminatedContract.gameinCustomerId).name;
                string[] param = {gameinCustomerName, translatedProductName};
                NotificationsController.Instance.AddNewNotification("notification_terminate_contract", param);
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
            string productName = GameDataManager.Instance.GetProductName(contractFinalizedResponse.contract.productId);
            string translatedProductName =
                LocalizationManager.GetLocalizedValue("product_" + productName,
                    LocalizationManager.GetCurrentLanguage());
            string gameinCustomerName = GameDataManager.Instance.GetGameinCustomerById(contractFinalizedResponse.contract.gameinCustomerId).name;
            string[] param = {gameinCustomerName, translatedProductName};
            NotificationsController.Instance.AddNewNotification("notification_contract_finalized", param);
        }
    }

    public void OnShowMoreDetailsButtonClick()
    {
        ContractMoreDetailsController.Instance.ShowMoreDetailsButtonClick(contractData);
    }
}
