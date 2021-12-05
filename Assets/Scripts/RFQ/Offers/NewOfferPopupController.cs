using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ProductionLine;
using RTLTMPro;

public class NewOfferPopupController : MonoBehaviour
{
    public static NewOfferPopupController Instance;

    public GameObject NewOfferPopupCanvas;

    public List<ProductDetailsSetter> ProductDetailsSetters;

    public TMP_InputField VolumeInputfield;
    public TMP_InputField PricePerUnitInputfield;
    public TMP_InputField TotalPriceInputfield;
    public Localize minMaxLocalize;

    private Utils.Product _selectedProduct = null;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewOfferResponseEvent += OnNewOfferResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewOfferResponseEvent -= OnNewOfferResponse;
    }

    private void OnNewOfferResponse(NewOfferResponse newOfferResponse)
    {
        _isSendingRequest = false;
        if (newOfferResponse.offer != null)
        {
            OffersController.Instance.AddMyOfferToList(newOfferResponse.offer);

            NewOfferPopupCanvas.SetActive(false);
        }
        else
        {
            NewOfferPopupCanvas.SetActive(false);
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenNewOfferPopupClick()
    {
        _isSendingRequest = false;

        ClearInputFields();

        SetProducts();
        
        minMaxLocalize.GetComponent<RTLTextMeshPro>().text = "";

        NewOfferPopupCanvas.SetActive(true);
    }

    private void ClearInputFields()
    {
        VolumeInputfield.text = "";
        PricePerUnitInputfield.text = "";
        TotalPriceInputfield.text = "";
    }

    private void SetProducts()
    {
        int index = 0;
        foreach (Utils.Product product in GameDataManager.Instance.Products)
        {
            if (product.productType == Utils.ProductType.SemiFinished)
            {
                bool hasThisProductCategoryProductionLine = ProductionLinesDataManager.Instance.CanUseProduct(product);

                ProductDetailsSetters[index].SetData(product, hasThisProductCategoryProductionLine, "NewOffer");
                index++;
            }
        }

        _selectedProduct = null;
    }

    public void OnProductClick(Utils.Product product)
    {
        _selectedProduct = product;

        minMaxLocalize.SetKey("min_max_text", _selectedProduct.minPrice.ToString(), _selectedProduct.maxPrice.ToString());
    }

    public void OnVolumeOrPriceValueChange()
    {
        string volume = VolumeInputfield.text;
        string price = PricePerUnitInputfield.text;
        if (string.IsNullOrEmpty(volume) || string.IsNullOrEmpty(price))
        {
            return;
        }

        TotalPriceInputfield.text = (int.Parse(volume) * float.Parse(price)).ToString("0.00");
    }

    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }

        string volume = VolumeInputfield.text;
        string price = PricePerUnitInputfield.text;
        if (string.IsNullOrEmpty(volume) || string.IsNullOrEmpty(price) || _selectedProduct == null)
        {
            DialogManager.Instance.ShowErrorDialog(_selectedProduct == null ? "no_product_selected_error" : "empty_input_field_error");
            return;
        }

        var parsedVolume = int.Parse(volume);
        var parsedCostPerUnit = float.Parse(price);
        var product = _selectedProduct;
        
        if (parsedCostPerUnit > product.maxPrice || parsedCostPerUnit < product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }

        if (MainHeaderManager.Instance.Money < parsedCostPerUnit * parsedVolume)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
            return;
        }

        _isSendingRequest = true;
        NewOfferRequest newOfferRequest = new NewOfferRequest(RequestTypeConstant.NEW_OFFER, new Utils.Offer()
        {
            productId = product.id,
            volume = parsedVolume,
            costPerUnit = parsedCostPerUnit,
        });
        RequestManager.Instance.SendRequest(newOfferRequest);
    }
}
