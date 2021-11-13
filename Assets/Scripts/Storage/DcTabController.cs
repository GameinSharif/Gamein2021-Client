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

    public RTLTextMeshPro dcName;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;
    private Utils.Product _currentSelectedProduct;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeDcProduct, 25);
        _itemControllers = new List<StorageProductController>(25);
    }

    public void Initialize(Utils.Storage dc)
    {
        _dc = dc;
        dcName.text = "DC " + _dc.DCId;
        
        _itemControllers.Clear();
        _pool.RemoveAll();
        foreach (Utils.StorageProduct storageProduct in _dc.storageProducts)
        {
            _pool.Add(new Tuple<Utils.Product, int>(GameDataManager.Instance.GetProductById(storageProduct.productId), storageProduct.amount));
        }
        
        RebuildListLayout();
        
        OnClosePopupButtonClicked();
    }

    public void OnDcProductClicked(Utils.Product product)
    {
        _currentSelectedProduct = product;
        actionPopup.SetActive(true);
    }

    private void InitializeDcProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var (product, amount) = productTuple;
        
        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(product, Utils.StorageType.DC);

        controller.name.text = product.name;
        controller.available.text = amount.ToString();
        
        //TODO get coming amount
        int coming = 0;
        controller.coming.text = coming.ToString();
        controller.total.text = (coming + amount).ToString();
        
        _itemControllers.Add(controller);
    }

    public void OnSendButtonClicked()
    {
        //TODO send request to server
        //We have the storage product index from _currentSelectedProduct
        //and we have the amount from amountInputField
    }

    public void OnRemoveButtonClicked()
    {
        //TODO send removeRequest to server
    }

    public void OnClosePopupButtonClicked()
    {
        actionPopup.SetActive(false);
        amountInputField.text = "";
        _currentSelectedProduct = null;
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
                    int coming = int.Parse(controller.coming.OriginalText);
                    controller.total.text = (coming + storageProduct.amount).ToString();
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