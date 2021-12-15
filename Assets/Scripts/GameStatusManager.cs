using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    public static GameStatusManager Instance;

    public GameObject GameStatusPopupCanvas;
    public GameObject CloseButtonGameObject;
    public GameObject background;
    public Localize textLocalize;

    [HideInInspector] public Utils.GameStatus GameStatus = Utils.GameStatus.RUNNING;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnUpdateGameStatusResponseEvent += OnUpdateGameStatusResponseReceived;
        EventManager.Instance.OnBanResponseEvent += OnBanResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateGameStatusResponseEvent -= OnUpdateGameStatusResponseReceived;
        EventManager.Instance.OnBanResponseEvent -= OnBanResponseReceived;
    }

    private void OnUpdateGameStatusResponseReceived(UpdateGameStatusResponse updateGameStatusResponse)
    {
        if (GameStatus == updateGameStatusResponse.gameStatus)
        {
            return;
        }

        OpenGameStatusPopup(updateGameStatusResponse.gameStatus);
    }

    public void OpenGameStatusPopup(Utils.GameStatus gameStatus)
    {
        textLocalize.SetKey("game_status_" + gameStatus);

        switch (gameStatus)
        {
            case Utils.GameStatus.RUNNING:
            case Utils.GameStatus.PAUSED:
            case Utils.GameStatus.AUCTION:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(true);
                background.SetActive(false);
                break;
            case Utils.GameStatus.NOT_STARTED:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(false);
                background.SetActive(false);
                break;
            case Utils.GameStatus.FINISHED:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(false);
                background.SetActive(true);
                break;
        }

        GameStatus = gameStatus;
    }
    
    private void OnBanResponseReceived(BanResponse banResponse)
    {
        int minuets = banResponse.minutes;
        if (minuets == 2000000)
        {
            textLocalize.SetKey("ban_bankrupt");
        }
        else
        {
            textLocalize.SetKey("ban_message", minuets.ToString());
        }
        
        GameStatusPopupCanvas.SetActive(true);
        CloseButtonGameObject.SetActive(false);
        background.SetActive(true);

    }
}
