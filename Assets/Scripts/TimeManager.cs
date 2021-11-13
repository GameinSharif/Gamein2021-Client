using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private CustomDateTime _serverTime;
    private TimeSpan _diff;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnServerTimeResponseEvent += OnServerTimeResponseReceived;
    }

    private void OnServerTimeResponseReceived(ServerTimeResponse serverTimeResponse)
    {
        _serverTime = serverTimeResponse.serverTime;
        _diff = DateTime.Now.Subtract(_serverTime.ToDateTime());
    }

    public CustomDateTime CurrentServerTime => new CustomDateTime(DateTime.Now.Subtract(_diff));
}