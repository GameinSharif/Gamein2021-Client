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
    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<string>(selector, tabPrefab, TabInitializer, 5);
    }

    private void Start()
    {
        // Test();
        Initialize();
    }

    private void Test()
    {
        var storage1 = new Utils.Storage
        {
            DCId = 1,
            type = Utils.StorageType.DC,
            storageProducts = new List<Utils.StorageProduct>
            {
                new Utils.StorageProduct {productId = 1, amount = 20},
                new Utils.StorageProduct {productId = 2, amount = 60},
            }
        };
        
        var storage2 = new Utils.Storage
        {
            DCId = 2,
            type = Utils.StorageType.WAREHOUSE,
            storageProducts = new List<Utils.StorageProduct>
            {
                new Utils.StorageProduct {productId = 3, amount = 30},
                new Utils.StorageProduct {productId = 4, amount = 40},
                new Utils.StorageProduct {productId = 1, amount = 10},
                new Utils.StorageProduct {productId = 2, amount = 20},
            }
        };
        
        StorageManager.Instance.Storages.Add(storage1);
        StorageManager.Instance.Storages.Add(storage2);

        GameDataManager.Instance.Products = new List<Utils.Product>
        {
            new Utils.Product {id = 1, productType = Utils.ProductType.RawMaterial, name = "Water"},
            new Utils.Product {id = 2, productType = Utils.ProductType.SemiFinished, name = "Sugar Water"},
            new Utils.Product {id = 3, productType = Utils.ProductType.Finished, name = "Coca-Cola"},
            new Utils.Product {id = 4, productType = Utils.ProductType.Finished, name = "Pepsi"},
        };
    }

    // re-creates tabs
    public void Initialize()
    {
        dcTabView.SetActive(false);
        warehouseTabView.SetActive(false);
        
        _pool.RemoveAll();
        foreach (var storage in StorageManager.Instance.Storages)
        {
            _pool.Add(storage.type + " " + storage.DCId);
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
        switch (storage.type)
        {
            case Utils.StorageType.DC:
                warehouseTabView.SetActive(false);
                dcTabView.SetActive(true);
                DcTabController.Instance.Initialize(storage);
                break;
            case Utils.StorageType.WAREHOUSE:
                dcTabView.SetActive(false);
                warehouseTabView.SetActive(true);
                WarehouseTabController.Instance.Initialize(storage);
                break;
        }
    }
}