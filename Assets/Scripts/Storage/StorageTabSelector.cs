using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageTabSelector : MonoBehaviour
{
    public static StorageTabSelector Instance;

    public GameObject dcTabView;
    public GameObject warehouseTabView;
    
    public Transform selector;
    public GameObject tabPrefab;

    private PoolingSystem<string> _pool;
    private int? _currentSelectedStorageId = null;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<string>(selector, tabPrefab, TabInitializer, 5);
    }

    // re-creates tabs
    public void Initialize()
    {
        dcTabView.SetActive(false);
        warehouseTabView.SetActive(false);
        
        _pool.RemoveAll();
        foreach (var storage in StorageManager.Instance.Storages)
        {
            _pool.Add(storage.dc + " " + storage.id);
        }
    }

    private void TabInitializer(GameObject theGameObject, int index, string tabName)
    {
        var controller = theGameObject.GetComponent<StorageTabItemController>();
        controller.SetIndex(index);
        controller.name.text = tabName;

        theGameObject.GetComponent<Toggle>().group = selector.gameObject.GetComponent<ToggleGroup>();
    }

    public void OnTabClicked(int index)
    {
        var storage = StorageManager.Instance.Storages[index];
        _currentSelectedStorageId = storage.id;

        if (storage.dc)
        {
            warehouseTabView.SetActive(false);
            dcTabView.SetActive(true);
            DcTabController.Instance.Initialize(storage);
        }
        else
        {
            dcTabView.SetActive(false);
            warehouseTabView.SetActive(true);
            WarehouseTabController.Instance.Initialize(storage);
        }
    }

    public void ApplyStockChangeToUI(Utils.Storage updatedStorage, Utils.StorageProduct updatedProduct)
    {
        if (_currentSelectedStorageId == null || _currentSelectedStorageId != updatedStorage.id)
        {
            return;
        }

        if (updatedStorage.dc && dcTabView.activeInHierarchy)
        {
            DcTabController.Instance.ChangeProductInList(updatedProduct);
        } 
        else if (!updatedStorage.dc && warehouseTabView.activeInHierarchy)
        {
            WarehouseTabController.Instance.ChangeProductInList(updatedProduct);
        }
    }
}