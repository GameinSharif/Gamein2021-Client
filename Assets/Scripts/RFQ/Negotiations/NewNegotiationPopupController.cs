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
            //TODO show error
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
        _isSendingRequest = true;

        string amount = AmountInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(price))
        {
            //TODO show error
            return;
        }

        NewProviderNegotiationRequest newProviderNegotiationRequest = new NewProviderNegotiationRequest(RequestTypeConstant.NEW_PROVIDER_NEGOTIATION, _provider.id, int.Parse(amount), float.Parse(price));
        RequestManager.Instance.SendRequest(newProviderNegotiationRequest);
    }
}
