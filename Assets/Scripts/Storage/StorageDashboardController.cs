using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageDashboardController : MonoBehaviour
{
    public static StorageDashboardController Instance;

    public GameObject dcTabView;
    public GameObject warehouseTabView;
    
    public Transform dcListScrollPanel;
    public GameObject dcListItemPrefab;
    public ToggleGroup dcListToggleGroup;

    private PoolingSystem<Utils.Storage> _pool;
    private int? _currentSelectedStorageId = null;

    private Utils.Storage _warehouse;
    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Utils.Storage>(dcListScrollPanel, dcListItemPrefab, DcListItemInitializer, 5);
    }

    // re-creates dc list items
    public void Initialize()
    {
        _pool.RemoveAll();
        foreach (var storage in StorageManager.Instance.Storages)
        {
            if (storage.dc)
            {
                _pool.Add(storage);
            }
            else
            {
                _warehouse = storage;
            }
        }
        RebuildListLayout();
    }

    private void DcListItemInitializer(GameObject theGameObject, int index, Utils.Storage storage)
    {
        var controller = theGameObject.GetComponent<DcListItemController>();
        controller.SetInfo(storage);

        theGameObject.GetComponent<Toggle>().group = dcListToggleGroup;
    }

    public void OnDcItemClicked(Utils.Storage storage)
    {
        warehouseTabView.SetActive(false);
        dcTabView.SetActive(true);
        DcTabController.Instance.Initialize(storage);
        _currentSelectedStorageId = storage.id;
    }

    public void WarehouseToggleOnValueChanged(bool value)
    {

        if (value)
        {
            dcTabView.SetActive(false);
            warehouseTabView.SetActive(true);
            WarehouseTabController.Instance.Initialize(_warehouse);
            _currentSelectedStorageId = _warehouse.id;
        }
        else
        {
            warehouseTabView.SetActive(false);
        }
        
    }

    public void StatsToggleOnValueChanged(bool value)
    {
        //TODO
    }

    public void ApplyStockChangeToUI(Utils.Storage updatedStorage, Utils.StorageProduct updatedProduct)
    {
        if (_currentSelectedStorageId == null || _currentSelectedStorageId != updatedStorage.id)
        {
            return;
        }

        if (updatedStorage.dc && dcTabView.activeSelf)
        {
            DcTabController.Instance.ChangeProductInList(updatedProduct);
        } 
        else if (!updatedStorage.dc && warehouseTabView.activeSelf)
        {
            WarehouseTabController.Instance.ChangeProductInList(updatedProduct);
        }
    }
    
    private void RebuildListLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(dcListScrollPanel as RectTransform);
    }
}