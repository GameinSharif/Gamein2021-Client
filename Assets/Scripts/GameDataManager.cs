using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public List<Utils.GameinCustomer> GameinCustomers;
    [HideInInspector] public List<Utils.Product> Products;

    [HideInInspector] public List<Utils.Auction> Auctions;
    [HideInInspector] public List<Utils.Factory> Factories;

    [HideInInspector] public List<Utils.WeekDemand> CurrentWeekDemands;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
        EventManager.Instance.OnGetCurrentWeekDemandsResponseEvent += OnGetCurrentWeekDemandsResponse;
        EventManager.Instance.OnGetAllAuctionsResponseEvent += OnGetAllAuctionsResponse;
        EventManager.Instance.OnGetAllFactoriesResponseEvent += OnGetAllFactoriesResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        GameinCustomers = getGameDataResponse.gameinCustomers;
        Products = getGameDataResponse.products;

        GameinCustomersManager.Instance.InitializeGameinCustomersInShop(GameinCustomers);
    }

    public void OnGetCurrentWeekDemandsResponse(GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse)
    {
        CurrentWeekDemands = getCurrentWeekDemandsResponse.currentWeekDemands;

        //TODO update for active demands of a gamein customer
    }

    public void OnGetAllAuctionsResponse(GetAllAuctionsResponse getAllAuctionsResponse)
    {
        Auctions = getAllAuctionsResponse.auctions;
        MapManager.Instance.UpdateAllOnMapMarkers();
    }
    
    public Utils.Auction GetAuctionById(int id)
    {
        return Auctions.First(a => a.id == id);
    }

    public void UpdateAuctionElement(Utils.Auction auction)
    {
        int index = Auctions.IndexOf(GetAuctionById(auction.id));
        Auctions[index] = auction;
    }

    public void OnGetAllFactoriesResponse(GetAllFactoriesResponse getAllFactoriesResponse)
    {
        Factories = getAllFactoriesResponse.factories;
        MapManager.Instance.UpdateAllOnMapMarkers();
    }
    
    public Utils.Factory GetFactoryById(int id)
    {
        return Factories.First(f => f.id == id);
    }
    
    public List<Utils.WeekDemand> GetCurrentWeekDemands(int gameinCustomerId)
    {
        return CurrentWeekDemands.Where(d => d.gameinCustomer.id == gameinCustomerId) as List<Utils.WeekDemand>;
    }
}
