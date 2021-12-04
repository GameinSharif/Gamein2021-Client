using RTLTMPro;
using UnityEngine;

public class OtherOfferItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro volume;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro total;
    public RTLTextMeshPro team;
    public Localize distance;
    public GameObject acceptButton;

    public Utils.Offer Offer => _offer;

    private Utils.Offer _offer;
    private Utils.Product _product;
    private Utils.Team _otherTeam;

    private bool _isSendingAccept = false;

    public void Initialize(Utils.Offer offer, bool isAccepted)
    {
        _offer = offer;
        _product = GameDataManager.Instance.GetProductById(offer.productId);
        _otherTeam = GameDataManager.Instance.GetTeamById(offer.teamId);
        
        product.SetKey("product_" + _product.name);
        volume.text = _offer.volume.ToString();
        costPerUnit.text = _offer.costPerUnit.ToString("0.00");
        total.text = (_offer.volume * _offer.costPerUnit).ToString("0.00");
        team.text = _otherTeam.teamName;
        distance.SetKey("distance", CalculateDistance().ToString());

        acceptButton.SetActive(!isAccepted);
    }

    private int CalculateDistance()
    {
        var source =
            GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.FACTORY, _otherTeam.factoryId);
        var destination = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        return TransportManager.Instance.GetTransportDistance(source, destination, Utils.VehicleType.TRUCK);
    }

    public void OnAcceptClicked()
    {
        if (_isSendingAccept)
        {
            return;
        }

        var availableAmount =
            StorageManager.Instance.GetProductAmountByStorage(StorageManager.Instance.GetWarehouse(), _product.id);

        if (availableAmount < _offer.volume)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_available_amount_error");
            return;
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingAccept = true;
                AcceptOfferRequest acceptOfferRequest = new AcceptOfferRequest(_offer.id);
                RequestManager.Instance.SendRequest(acceptOfferRequest);
            }
        });
    }
}