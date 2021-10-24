using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public List<Utils.GameinCustomer> GameinCustomers;
    [HideInInspector] public List<Utils.Product> Products;
    [HideInInspector] public List<Utils.ProductionLineTemplate> ProductionLineTemplates;

    [HideInInspector] public List<Utils.WeekDemand> CurrentWeekDemands;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
        EventManager.Instance.OnGetCurrentWeekDemandsResponseEvent += OnGetCurrentWeekDemandsResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        GameinCustomers = getGameDataResponse.gameinCustomers;
        Products = getGameDataResponse.products;
        ProductionLineTemplates = getGameDataResponse.productionLineTemplates;

        Debug.Log(ProductionLineTemplates);

        GameinCustomersManager.Instance.InitializeGameinCustomersInShop(GameinCustomers);
    }

    public void OnGetCurrentWeekDemandsResponse(GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse)
    {
        CurrentWeekDemands = getCurrentWeekDemandsResponse.currentWeekDemands;

        //TODO update for active demands of a gamein customer
    }

    public List<Utils.WeekDemand> GetCurrentWeekDemands(int gameinCustomerId)
    {
        return CurrentWeekDemands.Where(d => d.gameinCustomer.id == gameinCustomerId) as List<Utils.WeekDemand>;
    }
}
