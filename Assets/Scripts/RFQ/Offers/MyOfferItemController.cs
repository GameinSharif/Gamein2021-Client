using RTLTMPro;
using UnityEngine;

public class MyOfferItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro volume;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro total;
    public RTLTextMeshPro acceptDate;
    
    public Utils.Offer Offer => _offer;

    private Utils.Offer _offer;
    private Utils.Product _product;

    private bool _isSendingTerminate = false;

    public void Initialize(Utils.Offer offer)
    {
        _offer = offer;
        _product = GameDataManager.Instance.GetProductById(offer.productId);
        
        product.SetKey("product_" + _product.name);
        volume.text = _offer.volume.ToString();
        costPerUnit.text = _offer.costPerUnit.ToString();
        total.text = (_offer.volume * _offer.costPerUnit).ToString();
        acceptDate.text = offer.offerStatus == Utils.OfferStatus.ACCEPTED ? offer.acceptDate.ToString() : "";
    }

    public void SetAsAccepted(Utils.Offer offer)
    {
        _offer = offer;
        acceptDate.text = offer.acceptDate.ToString();
    }

    public void OnTerminateClicked()
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
                TerminateOfferRequest terminateOfferRequest = new TerminateOfferRequest(RequestTypeConstant.TERMINATE_OFFER, _offer.id);
                RequestManager.Instance.SendRequest(terminateOfferRequest);
            }
        });
    }
}