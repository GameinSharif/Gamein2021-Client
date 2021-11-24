using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class FinishedProductItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public Localize productNameLocalize;

    public GameObject showDemandsButtonGameObject;

    
    private Utils.Product _finishedProduct;

    public void SetInfo(int no, string productName)
    {
        this.no.text = no.ToString();
        productNameLocalize.SetKey("product_" + productName);
    }

    public void SetInfo(int no, Utils.Product finishedProduct)
    {
        _finishedProduct = finishedProduct;

        SetInfo(
            no: no,
            productName: finishedProduct.name
        );
        
        showDemandsButtonGameObject.SetActive(true);
    }

    public void OnShowDemandsButtonClicked()
    {
        FinishedProductDemandersPopupController.Instance.OnShowDemandersClick(_finishedProduct);
    }

}
