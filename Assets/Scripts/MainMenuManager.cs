using System;
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

    private void Start()
    {
        if (PlayerPrefs.GetInt("FactoryId") == 0)
        {
            CountrySelectionController.Instance.Initialize();
        }
    }

    public void OnOpenTradePageButtonClick()
    {
        GetOffersRequest getOffersRequest = new GetOffersRequest(RequestTypeConstant.GET_OFFERS);
        RequestManager.Instance.SendRequest(getOffersRequest);

        GetProvidersRequest getProvidersRequest = new GetProvidersRequest(RequestTypeConstant.GET_PROVIDERS);
        RequestManager.Instance.SendRequest(getProvidersRequest);

        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);
    }

    public void OnLoadMapSceneButtonClick()
    {
        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }

    //TODO for other pages
}
