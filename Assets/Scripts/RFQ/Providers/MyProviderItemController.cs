using System;
using RTLTMPro;
using UnityEngine;

public class MyProviderItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro capacity;
    public RTLTextMeshPro price;
    public RTLTextMeshPro total;
    public Localize storageLocalize;

    public Utils.Provider Provider => _provider;
    
    private Utils.Provider _provider;
    private Utils.Product _product;
    private Tuple<int, bool> _storageDetail;
    
    private bool _isSendingTerminate = false;
    
    public void Initialize(Utils.Provider provider)
    {
        _provider = provider;
        _product = GameDataManager.Instance.GetProductById(_provider.id);
        _storageDetail = StorageManager.Instance.GetStorageDetailsById(provider.storageId);
        
        product.SetKey("product_" + _product.name);
        capacity.text = _provider.capacity.ToString();
        price.text = _provider.price.ToString();
        total.text = (_provider.capacity * provider.price).ToString();

        if (_storageDetail.Item2)
        {
            storageLocalize.SetKey("provider_item_dc", _storageDetail.Item1.ToString());
        }
        else
        {
            storageLocalize.SetKey("provider_item_warehouse");
        }
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
}