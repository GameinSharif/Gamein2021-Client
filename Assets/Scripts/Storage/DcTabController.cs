using System;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DcTabController : MonoBehaviour
{
    public static DcTabController Instance;

    private PoolingSystem<Tuple<Utils.Product, int>> _pool;
    private List<StorageProductController> _itemControllers;
    private Utils.Storage _dc;

    public Transform scrollPanel;
    public GameObject storageProductPrefab;

    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeDcProduct, 10);
        _itemControllers = new List<StorageProductController>(10);
    }

    public void Initialize(Utils.Storage dc)
    {
        _dc = dc;
        
        _itemControllers.Clear();
        _pool.RemoveAll();
        foreach (Utils.StorageProduct storageProduct in _dc.products)
        {
            if (storageProduct.amount == 0) continue;
            
            _pool.Add(new Tuple<Utils.Product, int>(GameDataManager.Instance.GetProductById(storageProduct.productId), storageProduct.amount));
        }
        
        RebuildListLayout();
    }

    private void InitializeDcProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var (product, availableAmount) = productTuple;
        int comingAmount = TransportManager.Instance.CalculateInWayProductsAmount(_dc, product.id);

        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetData(product, Utils.StorageType.DC, availableAmount, comingAmount);
        
        _itemControllers.Add(controller);
    }

    public void OnProductTransportClicked(Utils.Product product)
    {
        StorageTransportPopupController.Instance.Initialize(_dc, product);
    }

    public void OnProductRemoveClicked(Utils.Product product)
    {
        RemoveProductController.Instance.Initialize(_dc, product);
    }

    public void ChangeProductInList(Utils.StorageProduct storageProduct)
    {
        foreach (var controller in _itemControllers)
        {
            if (controller.Product.id == storageProduct.productId)
            {
                if (storageProduct.amount == 0)
                {
                    _pool.Remove(controller.gameObject);
                    RebuildListLayout();
                }
                else
                {
                    controller.available.text = storageProduct.amount.ToString();
                    //int coming = int.Parse(controller.coming.OriginalText);
                    //controller.total.text = (coming + storageProduct.amount).ToString();
                }
                return;
            }
        }
        
        var product = GameDataManager.Instance.GetProductById(storageProduct.productId);
        _pool.Add(new Tuple<Utils.Product, int>(product, storageProduct.amount));
        RebuildListLayout();
    }
    
    private void RebuildListLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel as RectTransform);
    }
}