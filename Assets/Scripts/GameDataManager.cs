using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public List<RFQUtils.GameinCustomer> GameinCustomers;
    public List<RFQUtils.Product> Products;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetGameDataResponseEvent += OnGetGameDataResponse;
    }

    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        GameinCustomers = getGameDataResponse.gameinCustomers;
        Products = getGameDataResponse.products;
    }
}
