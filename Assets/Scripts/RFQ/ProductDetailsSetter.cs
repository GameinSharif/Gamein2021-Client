using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductDetailsSetter : MonoBehaviour
{
    public Image productImage;
    public Localize productNameLocalize;
    public Button button;

    private Utils.Product _product;
    private string _popupType;

    public void SetData(Utils.Product product, bool isAvailable, int index, string popupType)
    {
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        productNameLocalize.SetKey("product_" + product.name);
        button.interactable = isAvailable;

        _product = product;
        _popupType = popupType;
    }

    public void SetRawData(Utils.Product product)
    {
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        productNameLocalize.SetKey("product_" + product.name);
    }

    public void OnClicked()
    {
        switch (_popupType)
        {
            case "NewOffer":
                NewOfferPopupController.Instance.OnProductClick(_product);
                break;
            case "NewProvider":
                NewProviderPopupController.Instance.OnProductClick(_product);
                break;
        }
        Debug.Log("Helllllll");
    }
}
