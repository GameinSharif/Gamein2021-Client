using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public static bool IsLoadingMap;

    public GameObject MainMenuCanvasGameObject;
    public GameObject TradeParentGameObject;

    [HideInInspector] public bool IsInTradePage = false;

    private void Awake()
    {
        Instance = this;
        
        GetGameDataRequest getGameDataRequest = new GetGameDataRequest(RequestTypeConstant.GET_GAME_DATA);
        RequestManager.Instance.SendRequest(getGameDataRequest);

        GetTeamTransportsRequest getTeamTransportsRequest = new GetTeamTransportsRequest(RequestTypeConstant.GET_TEAM_TRANSPORTS);
        RequestManager.Instance.SendRequest(getTeamTransportsRequest);
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
        IsInTradePage = true;

        GetOffersRequest getOffersRequest = new GetOffersRequest(RequestTypeConstant.GET_OFFERS);
        RequestManager.Instance.SendRequest(getOffersRequest);

        GetProvidersRequest getProvidersRequest = new GetProvidersRequest(RequestTypeConstant.GET_PROVIDERS);
        RequestManager.Instance.SendRequest(getProvidersRequest);

        GetNegotiationsRequest getNegotiationsRequest = new GetNegotiationsRequest(RequestTypeConstant.GET_NEGOTIATIONS);
        RequestManager.Instance.SendRequest(getNegotiationsRequest);

        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);

        GetContractSuppliersRequest getContractSuppliersRequest = new GetContractSuppliersRequest(RequestTypeConstant.GET_CONTRACTS_WITH_SUPPLIER);
        RequestManager.Instance.SendRequest(getContractSuppliersRequest);

        GameinSuppliersController.Instance.UpdateSupplies();
    }

    public void OnOpenProductionLinesPageButtonClick()
    {
        var request = new GetProductionLinesRequest(RequestTypeConstant.GET_PRODUCTION_LINES);
        RequestManager.Instance.SendRequest(request);
    }

    public void OnLoadMapSceneButtonClick()
    {
        if (IsLoadingMap)
        {
            return;
        }

        IsLoadingMap = true;

        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<Canvas>() != null)
            {
                gameObject.SetActive(false);
            }
        }

        TradeParentGameObject.SetActive(false);

        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }

    public void OnBackToMainMenuFromTrade()
    {
        IsInTradePage = false;
    }

    //TODO for other pages
}
