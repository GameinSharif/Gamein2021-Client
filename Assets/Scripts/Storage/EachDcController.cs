using RTLTMPro;
using UnityEngine;

public class EachDcController : MonoBehaviour
{

    private bool _isClickable = false;
    private MapUtils.MapAgentMarker.AgentType _agentType;
    private Utils.DC _dcDto;

    public void SetValues(Utils.DC dcDto, MapUtils.MapAgentMarker.AgentType agentType)
    {
        _agentType = agentType;
        _dcDto = dcDto;

        if (_agentType == MapUtils.MapAgentMarker.AgentType.MyDistributionCenter ||
            _agentType == MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter)
        {
            _isClickable = true;
            sellPrice.text = _dcDto.sellPrice.ToString();
            buyPrice.text = _dcDto.buyingPrice.ToString();
        }
        
    }

    public void OnDcMarkerClicked()
    {
        
        if (!_isClickable)
        {
            return;
        }

        if (_agentType == MapUtils.MapAgentMarker.AgentType.MyDistributionCenter)
        {
            sellPopup.SetActive(true);
        }
        else if (_agentType == MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter)
        {
            buyPopup.SetActive(true);
        }
        
    }

    public GameObject sellPopup;
    public RTLTextMeshPro sellPrice;

    public void SellButtonPressed()
    {
        CloseAll();
        RequestManager.Instance.SendRequest(new SellDCRequest(RequestTypeConstant.SELL_DC, _dcDto.id));
    }
    

    [Space(10)]
    
    public GameObject buyPopup;
    public RTLTextMeshPro buyPrice;

    public void BuyButtonPressed()
    {
        CloseAll();
        RequestManager.Instance.SendRequest(new BuyDCRequest(RequestTypeConstant.BUY_DC, _dcDto.id));
    }

    private void CloseAll()
    {
        sellPopup.SetActive(false);
        buyPopup.SetActive(false);
    }

}