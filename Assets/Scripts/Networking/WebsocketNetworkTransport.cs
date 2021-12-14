using System;
using System.Collections;
using BestHTTP.WebSocket;
using UnityEngine;

public class WebsocketNetworkTransport : MonoBehaviour
{
    #region Singleton

    private static WebsocketNetworkTransport _instance;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        Connect();
    }

    public static WebsocketNetworkTransport Instance
    {
        get => _instance;
        set => _instance = value;
    }

    #endregion

    private string address = "ws://" + Constants.ServerIp + ":" + Constants.ServerPort;

    private WebSocket _webSocket;
    private bool _isOpened;
    private const int RELOAD_COUNT = 10;
    private int _count;
    public bool isReconnecting;

    public void Connect()
    {
        _webSocket = new WebSocket(new Uri(address + "/user"));

        _webSocket.OnOpen += OnOpen;
        _webSocket.OnMessage += OnMessageReceived;
        _webSocket.OnClosed += OnClosed;
        _webSocket.OnError += OnError;

        _webSocket.Open();
    }

    private void OnOpen(WebSocket webSocket)
    {
        Debug.Log("Opened");
        _count = 0;
        isReconnecting = false;
        _isOpened = true;

        ReconnectManager.Instance.OnReconnect();
    }

    private void OnMessageReceived(WebSocket webSocket, string message)
    {
        Debug.Log("received: " + message);
        RequestHandler.HandleServerResponse(message);
    }

    private void OnClosed(WebSocket webSocket, ushort code, string message)
    {
        Debug.Log("Websocket closed.");
        RequestManager.Instance.ResetStartReceived();
        _isOpened = false;
        if (_count < RELOAD_COUNT)
        {
            isReconnecting = true;
            _count++;
            StartCoroutine(WaitCoroutine());
        }
        else
        {
            isReconnecting = false;
            ReconnectManager.Instance.OnReconnectFail();
        }
    }

    private IEnumerator WaitCoroutine()
    {
        ReconnectManager.Instance.OpenReconnectPopup();
        yield return new WaitForSeconds(1.0f);
        Connect();
    }

    private void OnError(WebSocket webSocket, string reason)
    {
        Debug.Log("Websocket has error.");
        Debug.Log(reason);
        RequestManager.Instance.ResetStartReceived();
        _isOpened = false;
        if (_count < RELOAD_COUNT)
        {
            isReconnecting = true;
            _count++;
            StartCoroutine(WaitCoroutine());
        }
        else
        {
            isReconnecting = false;
            ReconnectManager.Instance.OnReconnectFail();
        }
    }

    public void GenerateData(string request)
    {
        _webSocket.Send(request);
    }

    private void OnApplicationQuit()
    {
        _webSocket.Close(1000, "Bye!");
        Debug.Log("Bye!");
    }

    public bool IsOpened
    {
        get => _isOpened;
        set => _isOpened = value;
    }

    public void AuthenticationError()
    {
        _count = RELOAD_COUNT;
    }

    public void CloseWebSocket()
    {
        _webSocket.Close();
        _count = RELOAD_COUNT;
    }
}