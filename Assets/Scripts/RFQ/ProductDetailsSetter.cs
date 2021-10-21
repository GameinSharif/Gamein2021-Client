using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductDetailsSetter : MonoBehaviour
{
    public Image ProductImage;
    public Localize ProductNameLocalize;
    public GameObject IsUnavailable;
    public Button Button;

    public void SetData(Utils.Product product, bool isAvailable, int index)
    {
        ProductImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        ProductNameLocalize.SetKey(product.name);
        IsUnavailable.SetActive(!isAvailable);

        Button.onClick.RemoveAllListeners();
        if (isAvailable)
        {
            Button.onClick.AddListener(() =>
            {
                NewProviderPopupController.Instance.OnProductClick(product.id, index);
            });
        }
    }
}
