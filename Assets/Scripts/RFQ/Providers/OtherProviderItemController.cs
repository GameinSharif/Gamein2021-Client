using System;
using RTLTMPro;
using UnityEngine;

public class OtherProviderItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro capacity;
    public RTLTextMeshPro price;
    public RTLTextMeshPro total;
    public RTLTextMeshPro team;
    public RTLTextMeshPro distance;

    public Utils.Provider Provider => _provider;
    
    private Utils.Provider _provider;
    private Utils.Product _product;
    private Utils.Team _otherTeam;
    private Tuple<int, bool> _storageDetail;

    public void Initialize(Utils.Provider provider)
    {
        _provider = provider;
        _product = GameDataManager.Instance.GetProductById(_product.id);
        _otherTeam = GameDataManager.Instance.GetTeamById(_provider.teamId);
        _storageDetail = StorageManager.Instance.GetStorageDetailsById(_provider.storageId);
        
        product.SetKey("product_" + _product.name);
        capacity.text = _provider.capacity.ToString();
        price.text = _provider.price.ToString();
        total.text = (_provider.capacity * provider.price).ToString();
        team.text = _otherTeam.teamName;
        distance.text = CalculateDistance() + " km";
    }

    public void OnNegotiationButtonClicked()
    {
        NewNegotiationPopupController.Instance.OpenNewNegotiationPopup(_provider);
    }
    
    private int CalculateDistance()
    {
        var source =
            GameDataManager.Instance.GetLocationByTypeAndId(
                _storageDetail.Item2 ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY,
                _storageDetail.Item1);
        var destination = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        return TransportManager.Instance.GetTransportDistance(source, destination, Utils.VehicleType.TRUCK);
    }
}