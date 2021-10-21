using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewProviderPopupController : MonoBehaviour
{
    public static NewProviderPopupController Instance;

    public GameObject NewProviderPopupCanvas;

    public List<ProductDetailsSetter> ProductDetailsSetters;
    public List<GameObject> IsSelectedGameObjects;

    public TMP_InputField CapacityInputfield;
    public TMP_InputField AveragePriceInputfield;
    public TMP_InputField MinPriceOnRecordInputfield;
    public TMP_InputField MaxPriceOnRecordInputfield;

    private void Awake()
    {
        Instance = this;
    }

    public void OnOpenNewProviderPopupClick()
    {
        //TODO clear inputfields

        SetProducts();

        NewProviderPopupCanvas.SetActive(true);
    }

    private void SetProducts()
    {
        int index = 0;
        foreach (Utils.Product product in GameDataManager.Instance.Products)
        {
            if (product.productType == Utils.ProductType.SemiFinished)
            {
                bool hasThisProductsProductionLine = true;
                //TODO

                ProductDetailsSetters[index].SetData(product, hasThisProductsProductionLine, index);
            }
        }
    }

    public void OnProductClick(int productId, int index)
    {
        DisableAllSelections();
        IsSelectedGameObjects[index].SetActive(true);

        //TODO Set Prices
    }

    public void DisableAllSelections()
    {
        foreach (GameObject gameObject in IsSelectedGameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDoneButtonClick()
    {
        //TODO send request
    }
}
