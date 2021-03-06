using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public static bool IsLoadingMap;

    public GameObject HeaderFooterGameObject;

    public List<GameObject> MainMenuTabCanvasGameobjects;
    public List<Image> MainMenuTabButtonsImages;

    public Sprite selectedTabSprite;
    public Sprite unselectedTabSprite;

    private void Awake()
    {
        Instance = this;
        
        GetGameDataRequest getGameDataRequest = new GetGameDataRequest(RequestTypeConstant.GET_GAME_DATA);
        RequestManager.Instance.SendRequest(getGameDataRequest);

        GetTeamTransportsRequest getTeamTransportsRequest = new GetTeamTransportsRequest(RequestTypeConstant.GET_TEAM_TRANSPORTS);
        RequestManager.Instance.SendRequest(getTeamTransportsRequest);

        GetAllChatsRequest getAllChatsRequest = new GetAllChatsRequest();
        RequestManager.Instance.SendRequest(getAllChatsRequest);

        GetStorageProductsRequest getStorageProductsRequest = new GetStorageProductsRequest(RequestTypeConstant.GET_STORAGES);
        RequestManager.Instance.SendRequest(getStorageProductsRequest);

        var request = new GetProductionLinesRequest(RequestTypeConstant.GET_PRODUCTION_LINES);
        RequestManager.Instance.SendRequest(request);

        GetAllWeeklyReportsRequest getAllWeeklyReportsRequest = new GetAllWeeklyReportsRequest(RequestTypeConstant.GET_ALL_WEEKLY_REPORTS);
        RequestManager.Instance.SendRequest(getAllWeeklyReportsRequest);
    }

    private void Start()
    {
        OpenPage(0);
        if (PlayerPrefs.GetInt("FactoryId") == 0)
        {
            DeactivateHeaderAndFooter();
        }
    }

    public void DeactivateHeaderAndFooter()
    {
        HeaderFooterGameObject.SetActive(false);
        CountrySelectionController.Instance.Initialize();
    }

    public void OpenPage(int index)
    {
        if (MainMenuTabButtonsImages[index].sprite == selectedTabSprite)
        {
            return;
        }

        DisableAll();

        switch (index)
        {
            case 0:
                OnLoadMapScene();
                break;
            case 1:
                OnOpenProductionLinesPage();
                break;
            case 2:
                OnOpenStoragePage();
                break;
            case 3:
                OnOpenMarketPage();
                break;
            case 4:
                OnOpenTransportPage();
                break;
            case 5:
                OnOpenReportsPage();
                break;
        }

        MainMenuTabCanvasGameobjects[index].SetActive(true);
        MainMenuTabButtonsImages[index].sprite = selectedTabSprite;
    }

    public void OnOpenMarketPage()
    {
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
        GameinCustomersController.Instance.UpdateDemands();

        RFQTabsManager.Instance.OnOpenMarketPage();
    }

    public void OnOpenProductionLinesPage()
    {
        var request = new GetProductionLinesRequest(RequestTypeConstant.GET_PRODUCTION_LINES);
        RequestManager.Instance.SendRequest(request);
    }

    public void OnOpenStoragePage()
    {
        GetStorageProductsRequest getStorageProductsRequest = new GetStorageProductsRequest(RequestTypeConstant.GET_STORAGES);
        RequestManager.Instance.SendRequest(getStorageProductsRequest);
    }

    public void OnOpenReportsPage()
    {
        GetAllWeeklyReportsRequest getAllWeeklyReportsRequest = new GetAllWeeklyReportsRequest(RequestTypeConstant.GET_ALL_WEEKLY_REPORTS);
        RequestManager.Instance.SendRequest(getAllWeeklyReportsRequest);

        WeeklyReportManager.Instance.OnOpenReportsPage();
    }

    public void OnOpenTransportPage()
    {
        GetTeamTransportsRequest getTeamTransportsRequest = new GetTeamTransportsRequest(RequestTypeConstant.GET_TEAM_TRANSPORTS);
        RequestManager.Instance.SendRequest(getTeamTransportsRequest);
    }

    public void OnLoadMapScene()
    {
        if (IsLoadingMap || MapManager.IsInMap)
        {
            return;
        }

        IsLoadingMap = true;
        //SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
        //MainMenuTabCanvasGameobjects[7].SetActive(true);
    }

    private void DisableAll()
    {
        //if (SceneManager.sceneCount > 1)
        //{
        //    SceneManager.UnloadSceneAsync("MapScene");
        //    MapManager.IsInMap = false;
        //}

        for (var i = 1; i < MainMenuTabCanvasGameobjects.Count; i++)
        {
            MainMenuTabCanvasGameobjects[i].SetActive(false);
        }

        foreach (Image image in MainMenuTabButtonsImages)
        {
            image.sprite = unselectedTabSprite;
        }
    }
}
