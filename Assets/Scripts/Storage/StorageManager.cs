using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    private List<Utils.Storage> storages;

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
        storages = getStorageProductsResponse.storageProducts;
    }
    
    
}