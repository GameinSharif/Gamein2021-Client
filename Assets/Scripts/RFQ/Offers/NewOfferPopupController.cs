using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ProductionLine;

public class NewOfferPopupController : MonoBehaviour
{
    public static NewOfferPopupController Instance;

    public GameObject NewOfferPopupCanvas;

    public List<ProductDetailsSetter> ProductDetailsSetters;
    public List<GameObject> IsSelectedGameObjects;

    public TMP_InputField VolumeInputfield;
    public TMP_InputField PricePerUnitInputfield;
    public TMP_InputField TotalPriceInputfield;
    public DatePicker OfferDeadline;

    private int _selectedProductId = 0;
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
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenNewOfferPopupClick()
    {
        _isSendingRequest = false;

        ClearInputFields();

        SetProducts();

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

                ProductDetailsSetters[index].SetData(product, hasThisProductCategoryProductionLine, index, "NewOffer");
                index++;
            }
        }
    }

    public void OnProductClick(int productId, int index)
    {
        DisableAllSelections();
        IsSelectedGameObjects[index].SetActive(true);
        _selectedProductId = productId;
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

        string volume = VolumeInputfield.text;
        string price = PricePerUnitInputfield.text;
        string date = OfferDeadline.currentSelectedDate.text;
        DateTime selectedDate = OfferDeadline.Value;
        if (string.IsNullOrEmpty(volume) || string.IsNullOrEmpty(price) || _selectedProductId == 0 || string.IsNullOrEmpty(date))
        {
            DialogManager.Instance.ShowErrorDialog(_selectedProductId == 0 ? "no_product_selected_error" : "empty_input_field_error");
            return;
        }

        var parsedVolume = int.Parse(volume);
        var parsedCostPerUnit = float.Parse(price);
        var product = GameDataManager.Instance.GetProductById(_selectedProductId);
        
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
            productId = _selectedProductId,
            volume = parsedVolume,
            costPerUnit = parsedCostPerUnit,
            offerDeadline = new CustomDate(selectedDate.Year, selectedDate.Month, selectedDate.Day),
        });
        RequestManager.Instance.SendRequest(newOfferRequest);
    }
}
