using System;
using ProductionLine;
using RTLTMPro;
using UnityEngine;

public class OtherProviderItemController : MonoBehaviour
{
    public Localize product;
    public RTLTextMeshPro capacity;
    public RTLTextMeshPro price;
    public RTLTextMeshPro team;
    public Localize distance;

    public Utils.Provider Provider => _provider;
    
    private Utils.Provider _provider;
    private Utils.Product _product;
    private Utils.Team _otherTeam;
    private Tuple<int, bool> _storageDetail;

    public void Initialize(Utils.Provider provider)
    {
        _provider = provider;
        _product = GameDataManager.Instance.GetProductById(_provider.productId);
        _otherTeam = GameDataManager.Instance.GetTeamById(_provider.teamId);
        _storageDetail = StorageManager.Instance.GetStorageDetailsById(_provider.storageId);
        
        product.SetKey("product_" + _product.name);
        capacity.text = _provider.capacity.ToString();
        price.text = _provider.price.ToString("0.00");

        team.text = _otherTeam.teamName;
        distance.SetKey("distance", CalculateDistance().ToString());
    }

    public void OnNegotiationButtonClicked()
    {
        if (ProductionLinesDataManager.Instance.CanUseProduct(_product))
        {
            NewNegotiationPopupController.Instance.OpenNewNegotiationPopup(_provider);
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog("can_not_use_this_product_error");
        }
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