﻿using System;
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

    public Utils.Storage GetStorageByBuildingIdAndType(int buildingId, bool isDC)
    {
        return Storages.Find(s => s.buildingId == buildingId && s.dc == isDC);
    }

    public void ChangeStockInStorage(int storageId, int productId, int amountToAddOrSubtract)
    {
        var storage = Storages.Find(st => st.id == storageId);
        if (storage == null)
        {
            DialogManager.Instance.ShowErrorDialog();
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
                DialogManager.Instance.ShowErrorDialog();
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

    public Utils.Storage GetWarehouse()
    {
        return Storages.Find(s => s.dc == false);
    }

    public int GetProductAmountByStorage(Utils.Storage storage, int productId)
    {
        foreach (Utils.StorageProduct storageProduct in storage.products)
        {
            if (storageProduct.productId == productId)
            {
                return storageProduct.amount;
            }
        }
        return 0;
    }

    public int CalculateAvailableCapacity(Utils.Storage storage, Utils.ProductType productType)
    {
        int availableCapacity = 0;
        if (!storage.dc)
        {
            if (productType == Utils.ProductType.RawMaterial)
            {
                availableCapacity = GameDataManager.Instance.GameConstants.rawMaterialCapacity;
            }
            else if (productType == Utils.ProductType.SemiFinished)
            {
                availableCapacity = GameDataManager.Instance.GameConstants.semiFinishedProductCapacity;
            }
            else if (productType == Utils.ProductType.Finished)
            {
                availableCapacity = GameDataManager.Instance.GameConstants.finishedProductCapacity;
            }

            availableCapacity -= CalculateCurrentOccupiedAmount(storage, productType);
        }
        else
        {
            Utils.DC dc = GameDataManager.Instance.GetDcById(storage.buildingId);
            availableCapacity = dc.capacity;
            if ((dc.type == Utils.DCType.SemiFinished && productType == Utils.ProductType.SemiFinished) || (dc.type == Utils.DCType.Finished && productType == Utils.ProductType.Finished))
            {
                availableCapacity -= CalculateCurrentOccupiedAmount(storage, productType);
            }
        }

        int inWayProductsAmount = TransportManager.Instance.CalculateInWayProductsAmount(storage, productType);

        return availableCapacity - inWayProductsAmount;
    }

    private int CalculateCurrentOccupiedAmount(Utils.Storage storage, Utils.ProductType productType)
    {
        int occupiedAmount = 0;
        foreach (Utils.StorageProduct storageProduct in storage.products)
        {
            Utils.Product storeProduct = GameDataManager.Instance.GetProductById(storageProduct.productId);
            if (storeProduct.productType == productType)
            {
                occupiedAmount -= storageProduct.amount * storeProduct.volumetricUnit;
            }
        }

        return occupiedAmount;
    }
}