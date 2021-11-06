using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public List<Utils.Team> Teams;
    [HideInInspector] public List<Utils.GameinCustomer> GameinCustomers;
    [HideInInspector] public List<Utils.Product> Products;

    [HideInInspector] public List<Utils.Auction> Auctions;
    [HideInInspector] public List<Utils.Factory> Factories;

    [HideInInspector] public List<Utils.WeekDemand> CurrentWeekDemands;

    [HideInInspector] public Utils.GameConstants GameConstants;

    public List<Sprite> ProductSprites;

    private int _auctionCurrentRound = 0;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
        EventManager.Instance.OnGetCurrentWeekDemandsResponseEvent += OnGetCurrentWeekDemandsResponse;
        EventManager.Instance.OnGetAllAuctionsResponseEvent += OnGetAllAuctionsResponse;
        EventManager.Instance.OnAuctionFinishedResponseEvent += OnAuctionFinishedResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        Teams = getGameDataResponse.teams;
        GameinCustomers = getGameDataResponse.gameinCustomers;
        Products = getGameDataResponse.products;

        Factories = getGameDataResponse.factories;

        GameConstants = getGameDataResponse.gameConstants;

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

        if (SceneManager.GetActiveScene().name == "MapScene")
            MapManager.Instance.UpdateAllAuctions();

        _auctionCurrentRound = 0;
        foreach(CustomDateTime customDateTime in GameConstants.AuctionRoundsStartTime)
        {
            if (DateTime.Now >= customDateTime.ToDateTime())
            {
                _auctionCurrentRound++;
            }
        }

        //TODO update auction remained time
    }

    public void OnAuctionFinishedResponse(AuctionFinishedResponse auctionFinishedResponse)
    {
        Teams = auctionFinishedResponse.teams;

        int teamId = PlayerPrefs.GetInt("TeamId");
        foreach (Utils.Team team in Teams)
        {
            if (team.id == teamId)
            {
                PlayerPrefs.SetInt("FactoryId", team.factoryId);
            }
        }

        SceneManager.UnloadScene("MapScene");
    }
    
    public Utils.Auction GetAuctionByFactoryId(int id)
    {
        return Auctions.FirstOrDefault(a => a.factoryId == id);
    }

    public void UpdateAuctionElement(Utils.Auction auction)
    {
        int index = Auctions.IndexOf(GetAuctionByFactoryId(auction.factoryId));
        if (index != -1)
        {
            Auctions[index] = auction;
        }
        else
        {
            Auctions.Add(auction);
        }

        MapManager.Instance.UpdateAllAuctions();
    }
    
    public Utils.Factory GetFactoryById(int id)
    {
        return Factories.First(f => f.id == id);
    }

    public Utils.Team GetTeamById(int id)
    {
        return Teams.First(t => t.id == id);
    }

    public List<Utils.WeekDemand> GetCurrentWeekDemands(int gameinCustomerId)
    {
        return CurrentWeekDemands.Where(d => d.gameinCustomer.id == gameinCustomerId) as List<Utils.WeekDemand>;
    }

    public string GetTeamName(int teamId)
    {
        foreach (Utils.Team team in Teams)
        {
            if (teamId == team.id)
            {
                return team.teamName;
            }
        }
        return "Team";
    }

    public Vector2 GetMyTeamLocaionOnMap()
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        Utils.Factory factory = GetFactoryById(GetTeamById(teamId).factoryId);
        return new Vector2((float)factory.latitude, (float)factory.longitude);
    }

    public Utils.Product GetProductById(int id)
    {
        return Products.First(p => p.id == id);
    }

    public bool IsAuctionOver()
    {
        return DateTime.Now > GameConstants.AuctionRoundsStartTime[GameConstants.AuctionRoundsStartTime.Count - 1].ToDateTime().AddSeconds(GameConstants.AuctionRoundDurationSeconds);
    }
}
