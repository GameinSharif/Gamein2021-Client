using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    //public Localize productNameLocalize;
    public RTLTextMeshPro transportationType;
    public RTLTextMeshPro transportationCost;
    public RTLTextMeshPro productName;
    public RTLTextMeshPro contractType;
    //public Localize OfferStatusLocalize;

    public GameObject DetailsButtonGameObject;
    public GameObject TerminateContractButtonGameObject;

    private Utils.ContractSupplier _contractSupplier;

    public void SetInfo(int no, string supplierName, string transportationType, string transportationCost, string productName, string contractType)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        //productNameLocalize.SetKey("product_" + productNameKey);
        this.transportationType.text = transportationType;
        this.transportationCost.text = transportationCost;
        this.productName.text = productName;
        this.contractType.text = contractType;
        
    }

    public void SetInfo(int no, Utils.ContractSupplier contractSupplier)
    {
        SetInfo(
            no: no,
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            //productNameKey: GameDataManager.Instance.GetProductById(offer.productId).name,
            transportationType: "transportationType", //TODO get from server
            transportationCost: "transportationCost", //TODO get from server
            productName: GameDataManager.Instance.GetProductName(contractSupplier.materialId), 
            contractType: contractSupplier.contractType.ToString()
        );

        if (contractSupplier.contractType == Utils.ContractType.LONGTERM)
        {
            TerminateContractButtonGameObject.SetActive(true);
            DetailsButtonGameObject.SetActive(true);
        }
        else
        {
            TerminateContractButtonGameObject.SetActive(false);
            DetailsButtonGameObject.SetActive(true);
        }

        _contractSupplier = contractSupplier;
    }


    public void OnDetailsButtonClicked()
    {
        // TODO
    }
    public void OnTerminateButtonClicked()
    {
        // TODO
    }
}