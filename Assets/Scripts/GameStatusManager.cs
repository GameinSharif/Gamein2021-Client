using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    public static GameStatusManager Instance;

    public GameObject GameStatusPopupCanvas;
    public GameObject CloseButtonGameObject;
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
        switch (updateGameStatusResponse.gameStatus)
        {
            case Utils.GameStatus.RUNNING:
                if (_gameStatus == Utils.GameStatus.RUNNING)
                {
                    return;
                }

                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(true);
                break;
            case Utils.GameStatus.PAUSED:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(true);
                break;
            case Utils.GameStatus.STOPPED:
                GameStatusPopupCanvas.SetActive(true);
                CloseButtonGameObject.SetActive(false);
                break;
        }

        _gameStatus = updateGameStatusResponse.gameStatus;
        textLocalize.SetKey("game_status_" + _gameStatus.ToString());
    }
}
