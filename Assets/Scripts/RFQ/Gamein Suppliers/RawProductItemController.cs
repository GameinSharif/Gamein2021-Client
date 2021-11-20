using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class RawProductItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public Localize productNameLocalize;

    public GameObject ShowSuppliesButtonGameObject;

    
    private Utils.Product _rawProduct;
    private List<Utils.WeekSupply> _supplies;

    public void SetInfo(int no, string productName)
    {
        this.no.text = no.ToString();
        productNameLocalize.SetKey("product_" + productName);
    }

    public void SetInfo(int no, Utils.Product rawProduct)
    {
        _rawProduct = rawProduct;

        SetInfo(
            no: no,
            productName: rawProduct.name
        );
        
        ShowSuppliesButtonGameObject.SetActive(true);
    }
    
    public void OnShowSuppliesButtonClicked()
    {
        RawProductSuppliersPopupController.Instance.OnShowSuppliersClick(_rawProduct);
    }
   
}
