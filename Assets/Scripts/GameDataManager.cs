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
    [HideInInspector] public List<Utils.Supplier> GameinSuppliers;
    [HideInInspector] public List<Utils.Product> Products;

    [HideInInspector] public List<Utils.Auction> Auctions;
    [HideInInspector] public List<Utils.Factory> Factories;

    [HideInInspector] public List<Utils.WeekDemand> CurrentWeekDemands;
    [HideInInspector] public List<Utils.WeekSupply> CurrentWeekSupplies;

    [HideInInspector] public Utils.GameConstants GameConstants;

    public List<Sprite> ProductSprites;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
        EventManager.Instance.OnGetCurrentWeekDemandsResponseEvent += OnGetCurrentWeekDemandsResponse;
        EventManager.Instance.OnGetCurrentWeekSuppliesResponseEvent += OnGetCurrentWeekSuppliesResponse;
        EventManager.Instance.OnGetAllAuctionsResponseEvent += OnGetAllAuctionsResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        Teams = getGameDataResponse.teams;
        GameinCustomers = getGameDataResponse.gameinCustomers;
        GameinSuppliers = getGameDataResponse.suppliers;
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

    public void OnGetCurrentWeekSuppliesResponse(GetCurrentWeekSuppliesResponse getCurrentWeekSuppliesResponse)
    {
        CurrentWeekSupplies = getCurrentWeekSuppliesResponse.currentWeekSupplies;
        //TODO update suppliers in the table
    }
    
    public void OnGetAllAuctionsResponse(GetAllAuctionsResponse getAllAuctionsResponse)
    {
        Auctions = getAllAuctionsResponse.auctions;

        if (SceneManager.GetActiveScene().name == "MapScene")
            MapManager.Instance.UpdateAllAuctions();
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

        MapManager.Instance.UpdateAuctionData(auction.factoryId);
    }
    
    public Utils.Factory GetFactoryById(int id)
    {
        return Factories.First(f => f.id == id);
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
    
    public string GetSupplierName(int supplierId)
    {
        foreach (Utils.Supplier supplier in GameinSuppliers)
        {
            if (supplierId == supplier.id)
            {
                return supplier.name;
            }
        }
        return "Supplier";
    }
    
    public string GetProductName(int productId)
    {
        foreach (Utils.Product product in Products)
        {
            if (productId == product.id)
            {
                return product.name;
            }
        }
        return "Product";
    }

    public Utils.Product GetProductById(int id)
    {
        return Products.First(p => p.id == id);
    }
}
