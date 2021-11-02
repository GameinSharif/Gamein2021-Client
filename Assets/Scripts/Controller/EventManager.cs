using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnConnectionResponseEvent += EncryptManager.OnConnectionResponse;
    }

    private void OnDisable()
    {
        OnConnectionResponseEvent -= EncryptManager.OnConnectionResponse;
    }

    public event Action<ConnectionResponse> OnConnectionResponseEvent;
    public void OnConnectionResponse(ConnectionResponse connectionResponse)
    {
        OnConnectionResponseEvent?.Invoke(connectionResponse);
    }

    public event Action<LoginResponse> OnLoginResponseEvent;  
    public void OnLoginResponse(LoginResponse loginResponse)
    {
        OnLoginResponseEvent?.Invoke(loginResponse);
    }

    public event Action<NewOfferResponse> OnNewOfferResponseEvent;
    public void OnNewOfferResponse(NewOfferResponse newOfferResponse)
    {
        OnNewOfferResponseEvent?.Invoke(newOfferResponse);
    }

    public event Action<GetOffersResponse> OnGetOffersResponseEvent;
    public void OnGetOffersResponse(GetOffersResponse getOffersResponse)
    {
        OnGetOffersResponseEvent?.Invoke(getOffersResponse);
    }

    public event Action<GetGameDataResponse> OnGetGameDataResponseEvent;
    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        OnGetGameDataResponseEvent?.Invoke(getGameDataResponse);
    }

    public event Action<GetCurrentWeekDemandsResponse> OnGetCurrentWeekDemandsResponseEvent;
    public void OnGetCurrentWeekDemandsResponse(GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse)
    {
        OnGetCurrentWeekDemandsResponseEvent?.Invoke(getCurrentWeekDemandsResponse);
    }

    public event Action<GetContractsResponse> OnGetContractsResponseEvent;
    public void OnGetContractsResponse(GetContractsResponse getContractsResponse)
    {
        OnGetContractsResponseEvent?.Invoke(getContractsResponse);
    }

    public event Action<GetProvidersResponse> OnGetProvidersResponseEvent;
    public void OnGetProvidersResponse(GetProvidersResponse getProvidersResponse)
    {
        OnGetProvidersResponseEvent?.Invoke(getProvidersResponse);
    }

    public event Action<BuyDCResponse> OnBuyDCResponseEvent;
    public void OnBuyDCResponse(BuyDCResponse buyDcResponse)
    {
        OnBuyDCResponseEvent?.Invoke(buyDcResponse);
    }
    
    public event Action<SellDCResponse> OnSellDCResponseEvent;
    public void OnSellDCResponse(SellDCResponse sellDcResponse)
    {
        OnSellDCResponseEvent?.Invoke(sellDcResponse);
    }

    public event Action<GetStorageProductsResponse> OnGetStorageProductsResponseEvent;

    public void OnGetStorageProductsResponse(GetStorageProductsResponse getStorageProductsResponse)
    {
        OnGetStorageProductsResponseEvent?.Invoke(getStorageProductsResponse);
    }

    public event Action<NewProviderResponse> OnNewProviderResponseEvent;
    public void OnNewProviderResponse(NewProviderResponse newProviderResponse)
    {
        OnNewProviderResponseEvent?.Invoke(newProviderResponse);
    }

    public event Action<BidForAuctionResponse> OnBidForAuctionResponseEvent;
    public void OnBidForAuctionResponse(BidForAuctionResponse bidForAuctionResponse)
    {
        OnBidForAuctionResponseEvent?.Invoke(bidForAuctionResponse);
    }

    public event Action<GetAllAuctionsResponse> OnGetAllAuctionsResponseEvent;
    public void OnGetAllAuctionsResponse(GetAllAuctionsResponse getAllAuctionsResponse)
    {
        OnGetAllAuctionsResponseEvent?.Invoke(getAllAuctionsResponse);
    }

    public event Action<NewMessageResponse> OnNewMessageResponseEvent;
    public void OnNewMessageResponse(NewMessageResponse newMessageResponse)
    {
        OnNewMessageResponseEvent?.Invoke(newMessageResponse);
    }

    public event Action<GetAllChatsResponse> OnGetAllChatsResponseEvent;
    public void OnGetAllChatsResponse(GetAllChatsResponse getAllChatsResponse)
    {
        OnGetAllChatsResponseEvent?.Invoke(getAllChatsResponse);
    }
}
