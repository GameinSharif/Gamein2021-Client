using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public GameObject mainMenu;
    public GameObject countrySelection;
    
    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("IsFirstTime") == 1)
        {
            mainMenu.SetActive(false);
            countrySelection.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
            countrySelection.SetActive(false);
        }
        
        GetGameDataRequest getGameDataRequest = new GetGameDataRequest(RequestTypeConstant.GET_GAME_DATA);
        RequestManager.Instance.SendRequest(getGameDataRequest);
    }

    public void OnOpenTradePageButtonClick()
    {
        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);

        GetProvidersRequest getProvidersRequest = new GetProvidersRequest(RequestTypeConstant.GET_PROVIDERS);
        RequestManager.Instance.SendRequest(getProvidersRequest);

        //TODO for offers, negotioations, ...
    }

    public void OnLoadMapSceneButtonClick()
    {
        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }

    //TODO for other pages
}
