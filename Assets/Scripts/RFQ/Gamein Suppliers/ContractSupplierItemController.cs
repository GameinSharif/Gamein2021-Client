using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierItemController : MonoBehaviour
{
    public RTLTextMeshPro supplierName;
    public Localize productNameLocalize;
    public RTLTextMeshPro contractDate;
    public RTLTextMeshPro pricePerUnit;
    public RTLTextMeshPro amount;
    public Localize transportType;
    public RTLTextMeshPro totalCost;

    public Color penaltyColor;

    public GameObject terminateButtonGameObject;
    public GameObject noTerminateTextGameObject;
    
    private Utils.ContractSupplier _contractSupplier;

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent += OnTerminateLongtermContractSupplierResponseReceived;
        EventManager.Instance.OnContractSupplierFinalizedResponseEvent += OnContractSupplierFinalizedRespinseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent -= OnTerminateLongtermContractSupplierResponseReceived;
        EventManager.Instance.OnContractSupplierFinalizedResponseEvent -= OnContractSupplierFinalizedRespinseReceived;
    }
    
    public void SetInfo(string supplierName, string productNameKey, CustomDate contractDate, float currentWeekPrice, int boughtAmount, Utils.VehicleType transportType, float totalCost)
    {
        this.supplierName.text = supplierName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.contractDate.text = contractDate.ToString();
        pricePerUnit.text = currentWeekPrice.ToString();
        amount.text = boughtAmount.ToString();
        this.transportType.SetKey(transportType.ToString());

        if (contractDate.ToDateTime() > MainHeaderManager.Instance.gameDate.ToDateTime()) //not started yet
        {
            this.totalCost.text = "-";
        }
        else if(_contractSupplier.noMoneyPenalty != 0) //not have enough money for this
        {
            this.totalCost.text = _contractSupplier.noMoneyPenalty.ToString();
            this.totalCost.color = penaltyColor;
        }
        else
        {
            this.totalCost.text = totalCost.ToString();
        }

        if (contractDate.ToDateTime() <= MainHeaderManager.Instance.gameDate.ToDateTime())
        {
            terminateButtonGameObject.SetActive(false);
            noTerminateTextGameObject.SetActive(true);
        }
    }

    public void SetInfo(Utils.ContractSupplier contractSupplier)
    {
        _contractSupplier = contractSupplier;

        SetInfo(
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            productNameKey: GameDataManager.Instance.GetProductById(contractSupplier.materialId).name,
            contractDate: contractSupplier.contractDate,
            currentWeekPrice: contractSupplier.pricePerUnit,
            boughtAmount: contractSupplier.boughtAmount,
            transportType: contractSupplier.transportType,
            totalCost: contractSupplier.transportationCost + contractSupplier.boughtAmount * contractSupplier.pricePerUnit
        );
    }

    public void OnTerminateButtonClicked()
    {
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                TerminateLongtermContractSupplierRequest terminateLongtermContractSupplier = new TerminateLongtermContractSupplierRequest(RequestTypeConstant.TERMINATE_LONGTERM_CONTRACT_WITH_SUPPLIER, _contractSupplier.id);
                RequestManager.Instance.SendRequest(terminateLongtermContractSupplier);
            }
        }, "terminate_contract", _contractSupplier.terminatePenalty.ToString());
    }
    
    private void OnTerminateLongtermContractSupplierResponseReceived(TerminateLongtermContractSupplierResponse terminateLongtermContractSupplierResponse)
    {
        if (_contractSupplier.id == terminateLongtermContractSupplierResponse.contractSupplier.id)
        {
            if (terminateLongtermContractSupplierResponse.result == "Successful")
            {
                MainHeaderManager.Instance.Money -= terminateLongtermContractSupplierResponse.contractSupplier.terminatePenalty;
                
                gameObject.SetActive(false);
            }
            else
            {
                DialogManager.Instance.ShowErrorDialog();
            }
        }
    }

    private void OnContractSupplierFinalizedRespinseReceived(ContractSupplierFinalizedResponse contractSupplierFinalizedResponse)
    {
        if (_contractSupplier.id == contractSupplierFinalizedResponse.contractSupplier.id)
        {
            SetInfo(contractSupplierFinalizedResponse.contractSupplier);

            //TODO notification
        }

    }
}
