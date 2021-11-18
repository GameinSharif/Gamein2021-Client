using System;
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
    private bool _isSendingRequest = false;

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
        _isSendingRequest = false;
        if (newProviderResponse.result == "Success")
        {
            ProvidersController.Instance.AddMyProviderToList(newProviderResponse.newProvider);

            NewProviderPopupCanvas.SetActive(false);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenNewProviderPopupClick()
    {
        _isSendingRequest = false;
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

        var (mean, max, min) = CalculateMeanMaxMinByProductId(productId);

        AveragePriceInputfield.text = mean.ToString();
        MaxPriceOnRecordInputfield.text = max.ToString();
        MinPriceOnRecordInputfield.text = min.ToString();
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
        if (_isSendingRequest)
        {
            return;
        }
        _isSendingRequest = true;

        string capacity = CapacityInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(capacity) || string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        if (_selectedProductId == 0)
        {
            DialogManager.Instance.ShowErrorDialog("no_product_selected_error");
            return;
        }

        NewProviderRequest newProviderRequest = new NewProviderRequest(RequestTypeConstant.NEW_PROVIDER, _selectedProductId, int.Parse(capacity), float.Parse(price));
        RequestManager.Instance.SendRequest(newProviderRequest);
    }

    private Tuple<float, float, float> CalculateMeanMaxMinByProductId(int productId)
    {
        float mean = 0, min = float.MaxValue, max = float.MinValue;
        int i = 0;

        foreach (var provider in ProvidersController.Instance.otherTeamsProviders)
        {
            if (provider.productId != productId) continue;
            if (provider.state == Utils.ProviderState.TERMINATED) continue;

            mean = (mean * i + provider.price) / (i + 1);
            if (provider.price > max)
            {
                max = provider.price;
            } else if (provider.price < min)
            {
                min = provider.price;
            }

            i++;
        }

        max = max == float.MinValue ? 0 : max;
        min = min == float.MaxValue ? 0 : min;

        return new Tuple<float, float, float>(mean, max, min);
    }
}
