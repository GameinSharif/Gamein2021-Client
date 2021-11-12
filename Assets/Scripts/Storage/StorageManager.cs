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
        Storages = getStorageProductsResponse.storageProducts;
    }

    public void ChangeStockInStorage(int dcId, int productId, int amountToAddOrSubtract)
    {
        var storage = Storages.Find(st => st.DCId == dcId);
        if (storage == null)
        {
            //show error?
            return;
        }

        var product = storage.storageProducts.Find(p => p.productId == productId);

        if (product == null)
        {
            product = new Utils.StorageProduct {productId = productId, amount = amountToAddOrSubtract};
            storage.storageProducts.Add(product);
        }
        else
        {
            product.amount += amountToAddOrSubtract;
            if (product.amount == 0)
            {
                storage.storageProducts.Remove(product);
            }
        }
        
        StorageTabSelector.Instance.ApplyStockChangeToUI(storage, product);
    }
    
}