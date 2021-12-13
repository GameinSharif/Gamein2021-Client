using System.Collections;
using UnityEngine;
using static Constants;

public class RequestManager : MonoBehaviour
{
    public static RequestManager Instance;

    private int _startReceived;

    private void Awake()
    {
        Instance = this;
    }

    public void SendRequest(RequestObject requestObject)
    {
        if (GameStatusManager.Instance.GameStatus == Utils.GameStatus.PAUSED && !isValidRequest(requestObject.requestTypeConstant))
        {
            GameStatusManager.Instance.OpenGameStatusPopup(GameStatusManager.Instance.GameStatus);
            return;
        }

        var request = JsonUtility.ToJson(requestObject);
        Debug.Log("sent: " + request);
        StartCoroutine(Send(request));
    }

    private bool isValidRequest(int requestType)
    {
        RequestTypeConstant requestTypeConstant = (RequestTypeConstant)requestType;
        switch (requestTypeConstant)
        {
            case RequestTypeConstant.LOGIN:
            case RequestTypeConstant.GET_OFFERS:
            case RequestTypeConstant.GET_GAME_DATA:
            case RequestTypeConstant.GET_CONTRACTS:
            case RequestTypeConstant.GET_NEGOTIATIONS:
            case RequestTypeConstant.GET_PROVIDERS:
            case RequestTypeConstant.GET_All_CHATS:
            case RequestTypeConstant.GET_PRODUCTION_LINES:
            case RequestTypeConstant.GET_TEAM_TRANSPORTS:
            case RequestTypeConstant.GET_CONTRACTS_WITH_SUPPLIER:
            case RequestTypeConstant.GET_LEADERBOARD:
            case RequestTypeConstant.GET_STORAGES:
            case RequestTypeConstant.GET_GAME_STATUS:
            case RequestTypeConstant.GET_ALL_WEEKLY_REPORTS:
                return true;
            default:
                return false;
        }
    }

    IEnumerator Send(string request)
    {
        WebsocketNetworkTransport.Instance.GenerateData(request);
        yield return null;
    }

    public void ResetStartReceived()
    {
        _startReceived = 0;
    }
}