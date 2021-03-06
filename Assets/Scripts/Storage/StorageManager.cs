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
        EventManager.Instance.OnRemoveProductResponseEvent += OnRemoveProductResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetStorageProductsResponseEvent -= OnGetStorageProductsResponse;
        EventManager.Instance.OnRemoveProductResponseEvent -= OnRemoveProductResponseReceived;
    }

    private void OnGetStorageProductsResponse(GetStorageProductsResponse getStorageProductsResponse)
    {
        Storages = getStorageProductsResponse.storages;

        StorageDashboardController.Instance.Initialize();
    }

    private void OnRemoveProductResponseReceived(RemoveProductResponse removeProductResponse)
    {
        RemoveProductController.Instance.OnResponse();
        
        if (removeProductResponse.storage == null)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }
        
        RemoveProductController.Instance.Close();

        var oldStorage = GetStorageByBuildingIdAndType(removeProductResponse.storage.buildingId, removeProductResponse.storage.dc);
        oldStorage.products = removeProductResponse.storage.products;

        if (oldStorage.dc)
        {
            DcTabController.Instance.Initialize(oldStorage);
            NotificationsController.Instance.AddNewNotification("notification_remove_product_dc");
        }
        else
        {
            WarehouseTabController.Instance.Initialize(oldStorage);
            NotificationsController.Instance.AddNewNotification("notification_remove_product_warehouse");
        }
    }

    public Utils.Storage GetStorageByBuildingIdAndType(int buildingId, bool isDC)
    {
        return Storages.Find(s => s.buildingId == buildingId && s.dc == isDC);
    }

    public Utils.Storage GetStorageById(int storageId)
    {
        return Storages.Find(s => s.id == storageId);
    }

    public void ChangeStockInStorage(int storageId, int productId, int amountToAddOrSubtract)
    {
        var storage = Storages.Find(st => st.id == storageId);
        if (storage == null)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        int availableCapacity = CalculateAvailableCapacity(storage, GameDataManager.Instance.GetProductById(productId).productType, false);

        if (availableCapacity < amountToAddOrSubtract)
        {
            amountToAddOrSubtract = availableCapacity;
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
        
        StorageDashboardController.Instance.ApplyStockChangeToUI(storage, product);
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

    public int CalculateAvailableCapacity(Utils.Storage storage, Utils.ProductType productType, bool withInWayProductsAmount)
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

        if (withInWayProductsAmount)
        {
            int inWayProductsAmount = TransportManager.Instance.CalculateInWayProductsAmount(storage, productType);
            availableCapacity -= inWayProductsAmount;
        }

        return availableCapacity;
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

    public Tuple<int, bool> GetStorageDetailsById(int id)
    {
        int buildingId = id > GameDataManager.Instance.Factories.Count
            ? id - GameDataManager.Instance.Factories.Count
            : id;
        bool dc = id > GameDataManager.Instance.Factories.Count;
        return new Tuple<int, bool>(buildingId, dc);
    }

    public float GetOccupiedAmount(Utils.Storage storage, Utils.Product product)
    {
        float volumetricAmount = 1f * GetProductAmountByStorage(storage, product.id) * product.volumetricUnit;
        if (storage.dc)
        {
            Utils.DC dc = GameDataManager.Instance.GetDcById(storage.buildingId);
            return volumetricAmount / dc.capacity * 100f;
        }
        else
        {
            int capacity = 0;
            switch (product.productType)
            {
                case Utils.ProductType.RawMaterial:
                    capacity = GameDataManager.Instance.GameConstants.rawMaterialCapacity;
                    break;
                case Utils.ProductType.SemiFinished:
                    capacity = GameDataManager.Instance.GameConstants.semiFinishedProductCapacity;
                    break;
                case Utils.ProductType.Finished:
                    capacity = GameDataManager.Instance.GameConstants.finishedProductCapacity;
                    break;
            }
            return volumetricAmount / capacity * 100f;
        }       
    }
}