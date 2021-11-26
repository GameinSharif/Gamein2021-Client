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

    private Utils.ProductType _currentSelectedType;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeWarehouseProduct, 10);
        _itemControllers = new List<StorageProductController>(25);
    }

    public void Initialize(Utils.Storage warehouse)
    {
        _warehouse = warehouse;
        
        OnRawChanged(true);
        
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

    public void InitializeWarehouseProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var (product, availableAmount) = productTuple;
        int comingAmount = TransportManager.Instance.CalculateInWayProductsAmount(_warehouse, product.id);

        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetData(product, Utils.StorageType.WAREHOUSE, availableAmount, comingAmount);

        _itemControllers.Add(controller);
    }
    
    public void OnProductTransportClicked(Utils.Product product)
    {
        StorageTransportPopupController.Instance.Initialize(_warehouse, product);
    }

    public void OnProductRemoveClicked(Utils.Product product)
    {
        RemoveProductController.Instance.Initialize(_warehouse, product);
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