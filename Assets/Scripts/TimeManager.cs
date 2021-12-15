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

        for(int i=0; i < GameDataManager.Instance.GameConstants.AuctionRoundsStartTime.Count; i++)
        {
            GameDataManager.Instance.GameConstants.AuctionRoundsStartTime[i] = new CustomDateTime(GameDataManager.Instance.GameConstants.AuctionRoundsStartTime[i].ToDateTime().Add(_diff));
        }

        Debug.Log(_diff);

        GameDataManager.Instance.SetAuctionCurrentRound();
        MapManager.Instance.Setup();
        BGM.instance.Setup();
        if (!GameDataManager.Instance.IsAuctionOver())
        {
            MainMenuManager.Instance.DeactivateHeaderAndFooter();
        }
    }

    public CustomDateTime CurrentServerTime => new CustomDateTime(DateTime.Now.Subtract(_diff));
}