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
    private Utils.Storage _warehouse;

    public Transform scrollPanel;
    public GameObject storageProductPrefab;

    public RTLTextMeshPro warehouseName;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;
    private int? _currentSelectedProductIndex;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeWarehouseProduct, 25);
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
        _pool.RemoveAll();
        foreach (Tuple<Utils.Product, int> productTuple in list)
        {
            _pool.Add(productTuple);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel as RectTransform);
    }
    
    public void OnWarehouseProductClicked(int index)
    {
        _currentSelectedProductIndex = index;
        actionPopup.SetActive(true);
    }

    public void InitializeWarehouseProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(index, Utils.StorageType.WAREHOUSE);

        var (product, amount) = productTuple;
        
        controller.name.text = product.name;
        controller.available.text = amount.ToString();
        
        //TODO get coming amount
        int coming = 0;
        controller.coming.text = coming.ToString();
        controller.total.text = (coming + amount).ToString();
    }
    
    public void OnSendButtonClicked()
    {
        //TODO send request to server
        //We have the storage product index from _currentSelectedProductIndex
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
        _currentSelectedProductIndex = null;
    }

    public void OnRawChanged(bool value)
    {
        if (!value) return;

        ResetList(FilterAndCreateList(Utils.ProductType.RawMaterial));
    }
    
    public void OnSemiChanged(bool value)
    {
        if (!value) return;

        ResetList(FilterAndCreateList(Utils.ProductType.SemiFinished));
    }
    
    public void OnFinishedChanged(bool value)
    {
        if (!value) return;
        
        ResetList(FilterAndCreateList(Utils.ProductType.Finished));
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
}