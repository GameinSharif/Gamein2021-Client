using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

        //TODO clear inputfields

        SetProducts();

        NewOfferPopupCanvas.SetActive(true);
    }

    private void SetProducts()
    {
        int index = 0;
        foreach (Utils.Product product in GameDataManager.Instance.Products)
        {
            if (product.productType == Utils.ProductType.SemiFinished)
            {
                bool hasThisProductCategoryProductionLine = true;
                //TODO

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
        _isSendingRequest = true;

        string volume = VolumeInputfield.text;
        string price = PricePerUnitInputfield.text;
        string date = OfferDeadline.currentSelectedDate.text;
        DateTime selectedDate = OfferDeadline.Value;
        if (string.IsNullOrEmpty(volume) || string.IsNullOrEmpty(price) || _selectedProductId == 0 || string.IsNullOrEmpty(date))
        {
            DialogManager.Instance.ShowErrorDialog(_selectedProductId == 0 ? "no_product_selected_error" : "empty_input_field_error");
            return;
        }

        NewOfferRequest newOfferRequest = new NewOfferRequest(RequestTypeConstant.NEW_OFFER, new Utils.Offer()
        {
            productId = _selectedProductId,
            volume = int.Parse(volume),
            costPerUnit = float.Parse(price),
            offerDeadline = new CustomDate(selectedDate.Year, selectedDate.Month, selectedDate.Day),
        });
        RequestManager.Instance.SendRequest(newOfferRequest);
    }
}
