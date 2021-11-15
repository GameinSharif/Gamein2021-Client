using RTLTMPro;
using UnityEngine;

public class EachDcController : MonoBehaviour
{
    public GameObject sellPopup;
    public RTLTextMeshPro sellPrice;
    [Space(10)]
    public GameObject buyPopup;
    public RTLTextMeshPro buyPrice;

    private bool _isClickable = false;
    private MapUtils.MapAgentMarker.AgentType _agentType;
    private Utils.DC _dc;

    private bool _isSendingRequest = false;

    public void SetValues(Utils.DC dc, MapUtils.MapAgentMarker.AgentType agentType)
    {
        _isSendingRequest = false;
        _agentType = agentType;
        _dc = dc;

        if (_agentType == MapUtils.MapAgentMarker.AgentType.MyDistributionCenter ||
            _agentType == MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter)
        {
            _isClickable = true;
            sellPrice.text = _dc.sellingPrice.ToString();
            buyPrice.text = _dc.buyingPrice.ToString();
        }
        else
        {
            _isClickable = false;
        }

        CloseAll();
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

    public void SellButtonPressed()
    {
        if (_isSendingRequest)
        {
            return;
        }
        _isSendingRequest = true;

        RequestManager.Instance.SendRequest(new SellDCRequest(RequestTypeConstant.SELL_DC, _dc.id));
    }

    public void BuyButtonPressed()
    {
        if (_isSendingRequest)
        {
            return;
        }
        _isSendingRequest = true;

        RequestManager.Instance.SendRequest(new BuyDCRequest(RequestTypeConstant.BUY_DC, _dc.id));
    }

    private void CloseAll()
    {
        sellPopup.SetActive(false);
        buyPopup.SetActive(false);
    }

}