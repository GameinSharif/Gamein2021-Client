using System;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DcTabController : MonoBehaviour
{
    public static DcTabController Instance;

    private PoolingSystem<Tuple<Utils.Product, int>> _pool;
    private Utils.Storage _dc;

    public Transform scrollPanel;
    public GameObject storageProductPrefab;

    public RTLTextMeshPro dcName;
    
    public GameObject actionPopup;
    public TMP_InputField amountInputField;
    private int? _currentSelectedProductIndex;

    private void Awake()
    {
        Instance = this;
        _pool = new PoolingSystem<Tuple<Utils.Product, int>>(scrollPanel, storageProductPrefab, InitializeDcProduct, 25);
    }

    public void Initialize(Utils.Storage dc)
    {
        _dc = dc;
        dcName.text = "DC " + _dc.DCId;
        
        _pool.RemoveAll();
        foreach (Utils.StorageProduct storageProduct in _dc.storageProducts)
        {
            _pool.Add(new Tuple<Utils.Product, int>(GameDataManager.Instance.GetProductById(storageProduct.productId), storageProduct.amount));
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel as RectTransform);
        
        OnClosePopupButtonClicked();
    }

    public void OnDcProductClicked(int index)
    {
        _currentSelectedProductIndex = index;
        actionPopup.SetActive(true);
    }

    private void InitializeDcProduct(GameObject theGameObject, int index, Tuple<Utils.Product, int> productTuple)
    {
        var controller = theGameObject.GetComponent<StorageProductController>();
        controller.SetInfo(index, Utils.StorageType.DC);

        var (product, amount) = productTuple;
        
        controller.name.text = product.name;
        controller.available.text = amount.ToString();
        
        //TODO get coming amount
        int coming = 0;
        controller.coming.text = coming.ToString();
        controller.total.text = (coming + amount).ToString();
    }

    public void OnSendButtonClicked()
    {
        //TODO send request to server
        //We have the storage product index from _currentSelectedProductIndex
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
        _currentSelectedProductIndex = null;
    }
}