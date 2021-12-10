using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawProductItemController : MonoBehaviour
{
    public Image ProductImage;
    public Localize ProductNameLocalize;
  
    private Utils.Product _rawProduct;

    public void SetInfo(Utils.Product rawProduct)
    {
        _rawProduct = rawProduct;

        ProductImage.sprite = GameDataManager.Instance.ProductSprites[rawProduct.id - 1];
        ProductNameLocalize.SetKey("product_" + rawProduct.name);
    }
    
    public void OnShowSuppliesButtonClicked()
    {
        SuppliersController.Instance.OnShowSuppliersClick(_rawProduct);
    }
   
}
