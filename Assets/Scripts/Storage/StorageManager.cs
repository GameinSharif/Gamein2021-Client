using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    public List<Utils.Storage> Storages
    {
        get;
        private set;
    }
    

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetStorageProductsResponseEvent += OnGetStorageProductsResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetStorageProductsResponseEvent -= OnGetStorageProductsResponse;
    }

    private void OnGetStorageProductsResponse(GetStorageProductsResponse getStorageProductsResponse)
    {
        Storages = getStorageProductsResponse.storages;

        StorageTabSelector.Instance.Initialize();
    }

    public void ChangeStockInStorage(int storageId, int productId, int amountToAddOrSubtract)
    {
        var storage = Storages.Find(st => st.id == storageId);
        if (storage == null)
        {
            //TODO show error?
            return;
        }

        var product = storage.products.Find(p => p.productId == productId);

        if (product == null)
        {
            if (amountToAddOrSubtract > 0)
            {
                product = new Utils.StorageProduct { productId = productId, amount = amountToAddOrSubtract };
                storage.products.Add(product);
            }
            else
            {
                //TODO error
                return;
            }
        }
        else
        {
            product.amount += amountToAddOrSubtract;
            if (product.amount == 0)
            {
                storage.products.Remove(product);
            }
        }
        
        StorageTabSelector.Instance.ApplyStockChangeToUI(storage, product);
    }
    
}