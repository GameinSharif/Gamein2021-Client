using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemoveProductController : MonoBehaviour
{
    public static RemoveProductController Instance;

    public GameObject popup;

    public Localize productName;
    public Image productImage;
    public TMP_InputField amountInputField;

    private Utils.Storage _storage;
    private Utils.Product _product;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(Utils.Storage storage, Utils.Product product)
    {
        _storage = storage;
        _product = product;
        
        productName.SetKey("product_" + _product.name);
        
        //TODO set product image
        
        popup.SetActive(true);
    }

    public void OnRemoveButtonClicked()
    {
        string amountText = amountInputField.text;
        if (string.IsNullOrWhiteSpace(amountText))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        int amount = int.Parse(amountText);
        if (amount <= 0)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_amount_error");
            return;
        }

        if (amount > StorageManager.Instance.GetProductAmountByStorage(_storage, _product.id))
        {
            DialogManager.Instance.ShowErrorDialog("dialog_popup_not_enough_product");
            return;
        }

        RemoveProductRequest removeProductRequest = new RemoveProductRequest(RequestTypeConstant.REMOVE_PRODUCT, _storage.dc, _storage.buildingId, _product.id, amount);
        RequestManager.Instance.SendRequest(removeProductRequest);
        
        popup.SetActive(false);
    }
}