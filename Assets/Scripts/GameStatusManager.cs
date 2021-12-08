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

    private Utils.GameStatus _gameStatus = Utils.GameStatus.RUNNING;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnUpdateGameStatusResponseEvent += OnUpdateGameStatusResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateGameStatusResponseEvent -= OnUpdateGameStatusResponseReceived;
    }

    private void OnUpdateGameStatusResponseReceived(UpdateGameStatusResponse updateGameStatusResponse)
    {

        textLocalize.SetKey("game_status_" + updateGameStatusResponse.gameStatus);

        switch (updateGameStatusResponse.gameStatus)
        {
            case Utils.GameStatus.RUNNING:
                if (_gameStatus == Utils.GameStatus.RUNNING)
                {
                    return;
                }

                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(true);
                background.SetActive(false);
                break;
            case Utils.GameStatus.PAUSED:
            case Utils.GameStatus.AUCTION:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(true);
                background.SetActive(false);
                break;
            case Utils.GameStatus.NOT_STARTED:
            case Utils.GameStatus.FINISHED:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(false);
                background.SetActive(true);
                break;
        }

        _gameStatus = updateGameStatusResponse.gameStatus;
    }
}
