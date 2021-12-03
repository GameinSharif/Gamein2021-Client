using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransportItemController : MonoBehaviour
{
    public ProductDetailsSetter productDetailsSetter;
    public RTLTextMeshPro productAmount;
    public Image sourceImage;
    public Localize sourceLocalize;
    public Image vehicle;
    public Image destinationImage;
    public Localize destinationLocalize;
    public RTLTextMeshPro startDate;
    public RTLTextMeshPro endDate;

    public Utils.Transport Transport => _transport;

    private Utils.Transport _transport;
    private bool _isGoing;

    public void Initialize(Utils.Transport transport, bool isGoing)
    {
        _transport = transport;
        _isGoing = isGoing;

        productDetailsSetter.SetImageOnly(_transport.contentProductId);
        productAmount.text = _transport.contentProductAmount.ToString();
        sourceImage.sprite = GetSourceSprite();
        sourceLocalize.SetKey("transport_" + _transport.sourceType, _transport.sourceId.ToString());
        vehicle.sprite = TransportsController.Instance.GetVehicleSprite(_transport.vehicleType);
        destinationImage.sprite = GetDestinationSprite();
        destinationLocalize.SetKey("transport_" + _transport.destinationType, _transport.destinationId.ToString());

        startDate.text = _transport.startDate.ToString();
        endDate.text = _transport.endDate.ToString();
    }

    private Sprite GetDestinationSprite()
    {
        switch (_transport.destinationType)
        {
            case Utils.TransportNodeType.SUPPLIER:
                return TransportsController.Instance.GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType.Supplier);
            case Utils.TransportNodeType.GAMEIN_CUSTOMER:
                return TransportsController.Instance.GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType.GameinCustomer);
            case Utils.TransportNodeType.DC:
                return TransportsController.Instance.GetSpriteByAgentType(_isGoing
                    ? MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter
                    : MapUtils.MapAgentMarker.AgentType.MyDistributionCenter);
            case Utils.TransportNodeType.FACTORY:
                return TransportsController.Instance.GetSpriteByAgentType(_isGoing
                    ? GetFactoryAgentType(_transport.destinationId)
                    : MapUtils.MapAgentMarker.AgentType.MyFactory);  
        }

        return null;
    }

    private Sprite GetSourceSprite()
    {
        switch (_transport.sourceType)
        {
            case Utils.TransportNodeType.SUPPLIER:
                return TransportsController.Instance.GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType.Supplier);
            case Utils.TransportNodeType.GAMEIN_CUSTOMER:
                return TransportsController.Instance.GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType.GameinCustomer);
            case Utils.TransportNodeType.DC:
                return TransportsController.Instance.GetSpriteByAgentType(_isGoing
                    ? MapUtils.MapAgentMarker.AgentType.MyDistributionCenter
                    : MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter);
            case Utils.TransportNodeType.FACTORY:
                return TransportsController.Instance.GetSpriteByAgentType(_isGoing
                    ? MapUtils.MapAgentMarker.AgentType.MyFactory
                    : GetFactoryAgentType(_transport.sourceId));
        }

        return null;
    }

    // chooses between other factory or different country factory
    private MapUtils.MapAgentMarker.AgentType GetFactoryAgentType(int id)
    {
        Utils.Factory factory = GameDataManager.Instance.GetFactoryById(id);
        int teamId = PlayerPrefs.GetInt("TeamId");
        Utils.Team myTeam = GameDataManager.Instance.GetTeamById(teamId);

        return myTeam.country == factory.country
            ? MapUtils.MapAgentMarker.AgentType.OtherFactory
            : MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory;
    }
}