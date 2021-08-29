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

    public void SendRequest<T>(RequestTypeConstant requestType, T requestData)
    {
        var requestObject = new RequestObject<T>(PlayerPrefs.GetInt("TeamID"), requestType, requestData);
        var request = JsonUtility.ToJson(requestObject);
        StartCoroutine(Send(request));
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