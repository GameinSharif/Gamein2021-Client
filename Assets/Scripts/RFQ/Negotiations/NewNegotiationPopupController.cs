using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewNegotiationPopupController: MonoBehaviour
{
    public static NewNegotiationPopupController Instance;

    public GameObject NewNegotiationPopupCanvas;

    public Image ProductImage;
    public Localize ProductNameLocalize;
    public TMP_InputField AmountInputfield;
    public TMP_InputField PriceInputfield;

    private Utils.Provider _provider;
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
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OpenNewNegotiationPopup(Utils.Provider provider)
    {
        _isSendingRequest = false;

        ProductImage.sprite = GameDataManager.Instance.ProductSprites[provider.productId - 1];
        ProductNameLocalize.SetKey("product_" + GameDataManager.Instance.Products[provider.productId - 1].name);
        _provider = provider;

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
        var product = GameDataManager.Instance.GetProductById(_provider.productId);

        if (parsedPrice > product.maxPrice || parsedPrice < product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }

        _isSendingRequest = true;
        NewProviderNegotiationRequest newProviderNegotiationRequest = new NewProviderNegotiationRequest(RequestTypeConstant.NEW_PROVIDER_NEGOTIATION, _provider.id, parsedAmount, parsedPrice);
        RequestManager.Instance.SendRequest(newProviderNegotiationRequest);
    }
}
