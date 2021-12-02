using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewNegotiationPopupController: MonoBehaviour
{
    public static NewNegotiationPopupController Instance;

    public GameObject NewNegotiationPopupCanvas;

    public TMP_InputField AmountInputfield;
    public TMP_InputField PriceInputfield;
    public ProductDetailsSetter productDetailsSetter;
    public Localize minMaxLocalize;

    private Utils.Provider _provider;
    private Utils.Product _product;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewProviderNegotiationResponseEvent += OnNewProviderNegotiationResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewProviderNegotiationResponseEvent -= OnNewProviderNegotiationResponse;
    }

    private void OnNewProviderNegotiationResponse(NewProviderNegotiationResponse newProviderNegotiationResponse)
    {
        _isSendingRequest = false;
        if (newProviderNegotiationResponse.negotiation != null)
        {
            NegotiationsController.Instance.AddNegotiationToList(newProviderNegotiationResponse.negotiation);

            NewNegotiationPopupCanvas.SetActive(false);
        }
        else
        {
            NewNegotiationPopupCanvas.SetActive(false);
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OpenNewNegotiationPopup(Utils.Provider provider)
    {
        _provider = provider;
        _isSendingRequest = false;

        _product = GameDataManager.Instance.GetProductById(_provider.productId);

        AmountInputfield.text = "";
        PriceInputfield.text = "";
        
        productDetailsSetter.SetRawData(_product);

        minMaxLocalize.SetKey("min_max_text", _product.minPrice.ToString(), _product.maxPrice.ToString());

        NewNegotiationPopupCanvas.SetActive(true);
    }

    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }

        string amount = AmountInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        var parsedAmount = int.Parse(amount);
        var parsedPrice = float.Parse(price);
        
        if (parsedPrice > _product.maxPrice || parsedPrice < _product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }

        if (parsedAmount < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_amount_error");
            return;
        }

        _isSendingRequest = true;
        NewProviderNegotiationRequest newProviderNegotiationRequest = new NewProviderNegotiationRequest(RequestTypeConstant.NEW_PROVIDER_NEGOTIATION, _provider.id, parsedAmount, parsedPrice);
        RequestManager.Instance.SendRequest(newProviderNegotiationRequest);
    }
}
