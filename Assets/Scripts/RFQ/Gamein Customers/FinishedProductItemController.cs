using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedProductItemController : MonoBehaviour
{
    public Image ProductImage;
    public Localize ProductNameLocalize;
    
    private Utils.Product _finishedProduct;

    public void SetInfo(Utils.Product rawProduct)
    {
        _finishedProduct = rawProduct;

        ProductImage.sprite = GameDataManager.Instance.ProductSprites[rawProduct.id - 1];
        ProductNameLocalize.SetKey("product_" + rawProduct.name);
    }

    public void OnShowDemandersButtonClicked()
    {
        CustomersController.Instance.OnShowDemandersClick(_finishedProduct);
    }

}
