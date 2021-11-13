﻿using System.Dynamic;
using RTLTMPro;
using UnityEngine;

public class StorageProductController : MonoBehaviour
{
    public RTLTextMeshPro name;
    public RTLTextMeshPro available;
    public RTLTextMeshPro coming;
    public RTLTextMeshPro total;

    private Utils.Product _product;
    private Utils.StorageType _storageType;

    public Utils.Product Product => _product;

    public void SetInfo(Utils.Product product, Utils.StorageType storageType)
    {
        _product = product;
        _storageType = storageType;
    }

    public void OnClicked()
    {
        switch (_storageType)
        {
            case Utils.StorageType.DC:
                DcTabController.Instance.OnDcProductClicked(_product);
                break;
            case Utils.StorageType.WAREHOUSE:
                WarehouseTabController.Instance.OnWarehouseProductClicked(_product);
                break;
        }
    }
}