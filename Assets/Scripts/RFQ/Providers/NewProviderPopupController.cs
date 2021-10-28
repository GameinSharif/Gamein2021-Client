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
    public TMP_InputField PriceInputfield;
    public TMP_InputField AveragePriceInputfield;
    public TMP_InputField MinPriceOnRecordInputfield;
    public TMP_InputField MaxPriceOnRecordInputfield;

    private int _selectedProductId = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewProviderResponseEvent += OnNewProviderResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewProviderResponseEvent -= OnNewProviderResponse;
    }

    private void OnNewProviderResponse(NewProviderResponse newProviderResponse)
    {
        if (newProviderResponse.result == "Success")
        {
            ProvidersController.Instance.AddMyProviderToList(newProviderResponse.newProvider);

            NewProviderPopupCanvas.SetActive(false);
        }
        else
        {
            //TODO show error
        }
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

                ProductDetailsSetters[index].SetData(product, hasThisProductsProductionLine, index, "NewProvider");
                index++;
            }
        }
    }

    public void OnProductClick(int productId, int index)
    {
        DisableAllSelections();
        IsSelectedGameObjects[index].SetActive(true);
        _selectedProductId = productId;

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
        string capacity = CapacityInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(capacity) || string.IsNullOrEmpty(price) || _selectedProductId == 0)
        {
            //TODO show error
            return;
        }

        NewProviderRequest newProviderRequest = new NewProviderRequest(RequestTypeConstant.NEW_PROVIDER, _selectedProductId, int.Parse(capacity), float.Parse(price));
        RequestManager.Instance.SendRequest(newProviderRequest);
    }
}
