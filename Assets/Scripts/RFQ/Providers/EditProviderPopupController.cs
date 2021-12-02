using TMPro;
using UnityEngine;

public class EditProviderPopupController : MonoBehaviour
{

    public static EditProviderPopupController Instance;
    
    public GameObject editProviderPopupCanvas;

    public TMP_InputField capacityInputField;
    public TMP_InputField priceInputField;
    public ProductDetailsSetter productDetailsSetter;
    public Localize minMaxLocalize;

    private Utils.Provider _provider;
    private Utils.Product _product;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenEditProviderPopup(Utils.Provider provider, Utils.Product product)
    {
        _provider = provider;
        _product = product;
        _isSendingRequest = false;

        capacityInputField.text = _provider.capacity.ToString();
        priceInputField.text = _provider.price.ToString("0.00");
        
        productDetailsSetter.SetRawData(_product);
        
        minMaxLocalize.SetKey("min_max_text", _product.minPrice.ToString(), _product.maxPrice.ToString());

        editProviderPopupCanvas.SetActive(true);
    }

    public void ClosePopup()
    {
        editProviderPopupCanvas.SetActive(false);
    }

    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }

        string capacity = capacityInputField.text;
        string price = priceInputField.text;
        if (string.IsNullOrEmpty(capacity) || string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        var parsedCapacity = int.Parse(capacity);
        var parsedPrice = float.Parse(price);
        
        if (parsedPrice > _product.maxPrice || parsedPrice < _product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }

        if (parsedCapacity < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_capacity_error");
            return;
        }

        _isSendingRequest = true;
        var editProviderRequest = new EditProviderRequest(_provider.id, parsedCapacity, parsedPrice);
        RequestManager.Instance.SendRequest(editProviderRequest);
    }
}