﻿using System;
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

    public RTLTextMeshPro warehouseName;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;
    private Utils.Product _currentSelectedProduct;
    private Utils.ProductType _currentSelectedType;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeWarehouseProduct, 25);
        _itemControllers = new List<StorageProductController>(25);
    }

    public void Initialize(Utils.Storage warehouse)
    {
        _warehouse = warehouse;
        warehouseName.text = "Warehouse " + _warehouse.DCId;
        
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
        var (product, amount) = productTuple;
        
        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(product, Utils.StorageType.WAREHOUSE);

        controller.name.text = product.name;
        controller.available.text = amount.ToString();
        
        //TODO get coming amount
        int coming = 0;
        controller.coming.text = coming.ToString();
        controller.total.text = (coming + amount).ToString();
        
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
        //TODO send removeRequest to server
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
        
        foreach (var storageProduct in _warehouse.storageProducts)
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