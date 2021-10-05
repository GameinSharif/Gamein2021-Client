using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    private void Awake()
    {
        Instance = this;

        GetGameDataRequest getGameDataRequest = new GetGameDataRequest(RequestTypeConstant.GET_GAME_DATA);
        RequestManager.Instance.SendRequest(getGameDataRequest);
    }

    public void OnOpenTradePageButtonClick()
    {
        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);

        //TODO for offers, providers, negotioations, ...
    }

    public void OnLoadMapSceneButtonClick()
    {
        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }

    //TODO for other pages
}
