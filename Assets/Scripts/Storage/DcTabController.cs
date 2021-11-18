﻿using System;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DcTabController : MonoBehaviour
{
    public static DcTabController Instance;

    private PoolingSystem<Tuple<Utils.Product, int>> _pool;
    private List<StorageProductController> _itemControllers;
    private Utils.Storage _dc;

    public Transform scrollPanel;
    public GameObject storageProductPrefab;

    public Localize dcNameLocalize;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;

    private Utils.Product _currentSelectedProduct;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeDcProduct, 25);
        _itemControllers = new List<StorageProductController>(25);
    }

    public void Initialize(Utils.Storage dc)
    {
        _dc = dc;
        dcNameLocalize.SetKey("Storage_Type_DC", _dc.buildingId.ToString());
        
        _itemControllers.Clear();
        _pool.RemoveAll();
        foreach (Utils.StorageProduct storageProduct in _dc.products)
        {
            _pool.Add(new Tuple<Utils.Product, int>(GameDataManager.Instance.GetProductById(storageProduct.productId), storageProduct.amount));
        }
        
        RebuildListLayout();
        
        OnClosePopupButtonClicked();
    }

    public void OnDcProductClicked(Utils.Product product)
    {
        _currentSelectedProduct = product;
        actionPopup.SetActive(true);
    }

    private void InitializeDcProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var (product, availableAmount) = productTuple;
        int comingAmount = TransportManager.Instance.CalculateInWayProductsAmount(_dc, product.id);

        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(product, Utils.StorageType.DC);
        controller.SetData(availableAmount, comingAmount);
        
        _itemControllers.Add(controller);
    }

    public void OnSendButtonClicked()
    {
        //TODO send request to server
        //We have the storage product index from _currentSelectedProduct
        //and we have the amount from amountInputField
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
        if (amount > StorageManager.Instance.GetProductAmountByStorage(_dc, _currentSelectedProduct.id))
        {
            DialogManager.Instance.ShowErrorDialog("dialog_popup_not_enough_product");
            return;
        }

        if (amount <= 0)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        RemoveProductRequest removeProductRequest = new RemoveProductRequest(RequestTypeConstant.REMOVE_PRODUCT, true, _dc.buildingId, _currentSelectedProduct.id, amount);
        RequestManager.Instance.SendRequest(removeProductRequest);
    }

    public void OnClosePopupButtonClicked()
    {
        actionPopup.SetActive(false);
        amountInputField.text = "";
        _currentSelectedProduct = null;
    }
    
    public void ChangeProductInList(Utils.StorageProduct storageProduct)
    {
        foreach (var controller in _itemControllers)
        {
            if (controller.Product.id == storageProduct.productId)
            {
                if (storageProduct.amount == 0)
                {
                    _pool.Remove(controller.gameObject);
                    RebuildListLayout();
                }
                else
                {
                    controller.available.text = storageProduct.amount.ToString();
                    int coming = int.Parse(controller.coming.OriginalText);
                    controller.total.text = (coming + storageProduct.amount).ToString();
                }
                return;
            }
        }
        
        var product = GameDataManager.Instance.GetProductById(storageProduct.productId);
        _pool.Add(new Tuple<Utils.Product, int>(product, storageProduct.amount));
        RebuildListLayout();
    }
    
    private void RebuildListLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel as RectTransform);
    }
}