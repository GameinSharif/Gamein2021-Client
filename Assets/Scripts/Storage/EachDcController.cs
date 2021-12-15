using RTLTMPro;
using System.Collections.Generic;
using UnityEngine;

public class EachDcController : MonoBehaviour
{
    public GameObject sellPopup;
    public Localize sellPriceLocalize;
    [Space(10)]
    public GameObject buyPopup;
    public Localize buyPriceLocalize;
    public Localize dcTypeLocalize;

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
            sellPriceLocalize.SetKey("dc_price", _dc.sellingPrice.ToString());
            buyPriceLocalize.SetKey("dc_price", _dc.buyingPrice.ToString());
            dcTypeLocalize.SetKey(_dc.type == Utils.DCType.SemiFinished ? "dc_type_semi" : "dc_type_finished");
        }
        else
        {
            _isClickable = false;
        }

        CloseAll();
    }

    public void OnDcMarkerClicked()
    {      
        if (!_isClickable || !GameDataManager.Instance.IsAuctionOver())
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

        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                List<Utils.Transport> transports = TransportManager.Instance.GetTransportsByDestinationTypeAndId(Utils.TransportNodeType.DC, _dc.id);
                if (transports == null || transports.Count != 0)
                {
                    DialogManager.Instance.ShowErrorDialog("dialog_popup_in_way_transport");
                    return;
                }

                _isSendingRequest = true;
                RequestManager.Instance.SendRequest(new SellDCRequest(RequestTypeConstant.SELL_DC, _dc.id));
                StartCoroutine(SetFalse());
            }
        });
    }

    public void BuyButtonPressed()
    {
        if (_isSendingRequest)
        {
            return;
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                if (MainHeaderManager.Instance.Money < _dc.buyingPrice)
                {
                    DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                    return;
                }

                _isSendingRequest = true;
                RequestManager.Instance.SendRequest(new BuyDCRequest(RequestTypeConstant.BUY_DC, _dc.id));
                StartCoroutine(SetFalse());
            }
        });
        
    }

    private void CloseAll()
    {
        sellPopup.SetActive(false);
        buyPopup.SetActive(false);
    }

    private IEnumerator<dynamic> SetFalse()
    {
        yield return new WaitForSeconds(5f);
        _isSendingRequest = false;
    }

}