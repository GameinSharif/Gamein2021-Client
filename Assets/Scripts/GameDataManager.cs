using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public List<RFQUtils.GameinCustomer> GameinCustomers;
    [HideInInspector] public List<RFQUtils.Product> Products;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        GameinCustomers = getGameDataResponse.gameinCustomers;
        Products = getGameDataResponse.products;

        Debug.Log(Products.Count);

        GameinCustomersManager.Instance.InitializeGameinCustomersInShop(GameinCustomers);
    }
}
