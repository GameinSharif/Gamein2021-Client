using System;
using RTLTMPro;
using UnityEngine;

public class MyProviderItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro capacity;
    public RTLTextMeshPro price;
    public Localize storageLocalize;

    public Utils.Provider Provider => _provider;
    
    private Utils.Provider _provider;
    private Utils.Product _product;
    private Tuple<int, bool> _storageDetail;
    
    private bool _isSendingTerminate = false;
    
    public void Initialize(Utils.Provider provider)
    {
        _provider = provider;
        _product = GameDataManager.Instance.GetProductById(_provider.productId);
        _storageDetail = StorageManager.Instance.GetStorageDetailsById(provider.storageId);
        
        product.SetKey("product_" + _product.name);
        capacity.text = _provider.capacity.ToString();
        price.text = _provider.price.ToString("0.00");

        if (_storageDetail.Item2)
        {
            storageLocalize.SetKey("provider_item_dc", _storageDetail.Item1.ToString());
        }
        else
        {
            storageLocalize.SetKey("provider_item_warehouse");
        }
    }

    public void UpdateEditedProvider(Utils.Provider provider)
    {
        _provider = provider;
        
        capacity.text = _provider.capacity.ToString();
        price.text = _provider.price.ToString("0.00");
    }

    public void OnRemoveClicked()
    {
        if (_isSendingTerminate)
        {
            return;
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingTerminate = true;
                var request = new RemoveProviderRequest(RequestTypeConstant.REMOVE_PROVIDER, _provider.id);
                RequestManager.Instance.SendRequest(request);
            }
        });
    }

    public void OnEditClicked()
    {
        EditProviderPopupController.Instance.OpenEditProviderPopup(_provider, _product);
    }
}