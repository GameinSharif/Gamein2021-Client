using System;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseTabController : MonoBehaviour
{
    public static WarehouseTabController Instance;

    private PoolingSystem<Tuple<Utils.Product, int>> _pool;
    private List<StorageProductController> _itemControllers;
    private Utils.Storage _warehouse;

    public Transform scrollPanel;
    public GameObject storageProductPrefab;

    public Localize warehouseNameLoccalize;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;

    private Utils.Product _currentSelectedProduct;
    private Utils.ProductType _currentSelectedType;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeWarehouseProduct, 25);
        _itemControllers = new List<StorageProductController>(25);
    }

    public void Initialize(Utils.Storage warehouse)
    {
        _warehouse = warehouse;
        warehouseNameLoccalize.SetKey("Storage_Type_WAREHOUSE");
        
        OnRawChanged(true);
        
        OnClosePopupButtonClicked();
    }

    private void ResetList(List<Tuple<Utils.Product, int>> list)
    {
        _itemControllers.Clear();
        _pool.RemoveAll();
        foreach (Tuple<Utils.Product, int> productTuple in list)
        {
            _pool.Add(productTuple);
        }

        RebuildListLayout();
    }
    
    public void OnWarehouseProductClicked(Utils.Product product)
    {
        _currentSelectedProduct = product;
        actionPopup.SetActive(true);
    }

    public void InitializeWarehouseProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var (product, availableAmount) = productTuple;
        int comingAmount = TransportManager.Instance.CalculateInWayProductsAmount(_warehouse, product.id);

        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(product, Utils.StorageType.WAREHOUSE);
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
        if (amount > StorageManager.Instance.GetProductAmountByStorage(_warehouse, _currentSelectedProduct.id))
        {
            DialogManager.Instance.ShowErrorDialog("dialog_popup_not_enough_product");
            return;
        }

        if (amount <= 0)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        RemoveProductRequest removeProductRequest = new RemoveProductRequest(RequestTypeConstant.REMOVE_PRODUCT, false, _warehouse.buildingId, _currentSelectedProduct.id, amount);
        RequestManager.Instance.SendRequest(removeProductRequest);
    }

    public void OnClosePopupButtonClicked()
    {
        actionPopup.SetActive(false);
        amountInputField.text = "";
        _currentSelectedProduct = null;
    }

    public void OnRawChanged(bool value)
    {
        if (!value) return;
        
        ResetList(FilterAndCreateList(Utils.ProductType.RawMaterial));
        _currentSelectedType = Utils.ProductType.RawMaterial;
    }
    
    public void OnSemiChanged(bool value)
    {
        if (!value) return;

        ResetList(FilterAndCreateList(Utils.ProductType.SemiFinished));
        _currentSelectedType = Utils.ProductType.SemiFinished;
    }
    
    public void OnFinishedChanged(bool value)
    {
        if (!value) return;
        
        ResetList(FilterAndCreateList(Utils.ProductType.Finished));
        _currentSelectedType = Utils.ProductType.Finished;
    }

    private List<Tuple<Utils.Product, int>> FilterAndCreateList(Utils.ProductType type)
    {
        var list = new List<Tuple<Utils.Product, int>>();
        
        foreach (var storageProduct in _warehouse.products)
        {
            var product = GameDataManager.Instance.GetProductById(storageProduct.productId);
            if (product.productType == type)
            {
                list.Add(new Tuple<Utils.Product, int>(product, storageProduct.amount));
            }
        }

        return list;
    }

    public void ChangeProductInList(Utils.StorageProduct storageProduct)
    {
        var product = GameDataManager.Instance.GetProductById(storageProduct.productId);

        if (_currentSelectedType != product.productType)
        {
            return;
        }
        
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
        
        _pool.Add(new Tuple<Utils.Product, int>(product, storageProduct.amount));
        RebuildListLayout();
    }

    private void RebuildListLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel as RectTransform);
    }
}