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
    
    private Utils.ContractSupplier _contractSupplier;

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent += OnTerminateLongtermContractSupplierResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent -= OnTerminateLongtermContractSupplierResponseReceived;
    }
    
    public void SetInfo(string supplierName, string productNameKey, CustomDate contractDate, float currentWeekPrice, int boughtAmount, Utils.VehicleType transportType, float totalCost)
    {
        this.supplierName.text = supplierName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.contractDate.text = contractDate.ToString();
        pricePerUnit.text = currentWeekPrice.ToString();
        amount.text = boughtAmount.ToString();
        this.transportType.SetKey(transportType.ToString());
        this.totalCost.text = totalCost.ToString();
    }

    public void SetInfo(Utils.ContractSupplier contractSupplier)
    {
        SetInfo(
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            productNameKey: GameDataManager.Instance.GetProductById(contractSupplier.materialId).name,
            contractDate: contractSupplier.contractDate,
            currentWeekPrice: contractSupplier.pricePerUnit,
            boughtAmount: contractSupplier.boughtAmount,
            transportType: contractSupplier.transportType,
            totalCost: contractSupplier.transportationCost + contractSupplier.boughtAmount * contractSupplier.pricePerUnit
        );

        _contractSupplier = contractSupplier;
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
        });
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
}
