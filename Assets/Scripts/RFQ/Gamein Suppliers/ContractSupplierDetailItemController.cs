using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierDetailItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro date;
    public RTLTextMeshPro amount;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro finalCost;

    //public Localize OfferStatusLocalize;
    
    private Utils.ContractSupplierDetail _contractSupplierDetail;

    public void SetInfo(int no, string date,  string amount, string costPerUnit, string finalCost)
    {
        this.no.text = no.ToString();
        this.date.text = date;
        this.amount.text = amount;
        this.costPerUnit.text = costPerUnit;
        this.finalCost.text = finalCost;
    }

    public void SetInfo(int no, Utils.ContractSupplierDetail contractSupplierDetail)
    {
        SetInfo(
            no: no,
            date: contractSupplierDetail.contractDate.ToString(),
            amount: contractSupplierDetail.boughtAmount.ToString(), 
            costPerUnit: contractSupplierDetail.pricePerUnit + "$",
            finalCost: (contractSupplierDetail.pricePerUnit * contractSupplierDetail.boughtAmount) + "$"
        );
        
        _contractSupplierDetail = contractSupplierDetail;
    }

}
