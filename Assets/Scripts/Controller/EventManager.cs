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

    public event Action<NewProviderResponse> OnNewProviderResponseEvent;
    public void OnNewProviderResponse(NewProviderResponse newProviderResponse)
    {
        OnNewProviderResponseEvent?.Invoke(newProviderResponse);
    }

    public event Action<GetNegotiationsResponse> OnGetNegotiationsResponseEvent;
    public void OnGetNegotiationsResponse(GetNegotiationsResponse getNegotiationsResponse)
    {
        OnGetNegotiationsResponseEvent?.Invoke(getNegotiationsResponse);
    }

    public event Action<NewProviderNegotiationResponse> OnNewProviderNegotiationResponseEvent;
    public void OnNewProviderNegotiationResponse(NewProviderNegotiationResponse newProviderNegotiationResponse)
    {
        OnNewProviderNegotiationResponseEvent?.Invoke(newProviderNegotiationResponse);
    }

    public event Action<EditNegotiationCostPerUnitResponse> OnEditNegotiationCostPerUnitResponseEvent;
    public void OnEditNegotiationCostPerUnitResponse(EditNegotiationCostPerUnitResponse editNegotiationCostPerUnitResponse)
    {
        OnEditNegotiationCostPerUnitResponseEvent?.Invoke(editNegotiationCostPerUnitResponse);
    }

    public event Action<RemoveProviderResponse> OnRemoveProviderResponseEvent;
    public void OnRemoveProviderResponse(RemoveProviderResponse removeProviderResponse)
    {
        OnRemoveProviderResponseEvent?.Invoke(removeProviderResponse);
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

    public event Action<AuctionFinishedResponse> OnAuctionFinishedResponseEvent;
    public void OnAuctionFinishedResponse(AuctionFinishedResponse auctionFinishedResponse)
    {
        OnAuctionFinishedResponseEvent?.Invoke(auctionFinishedResponse);
    }

    public event Action<TerminateOfferResponse> OnTerminateOfferResponseEvent;
    public void OnTerminateOfferResponse(TerminateOfferResponse terminateOfferResponse)
    {
        OnTerminateOfferResponseEvent?.Invoke(terminateOfferResponse);
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

    public event Action<GetTeamTransportsResponse> OnGetTeamTransportsResponseEvent;
    public void OnGetTeamTransportsResponse(GetTeamTransportsResponse getTeamTransportsResponse)
    {
        OnGetTeamTransportsResponseEvent?.Invoke(getTeamTransportsResponse);
    }

    public event Action<TransportStateChangedResponse> OnTransportStateChangedResponseEvent;
    public void OnTransportStateChangedResponse(TransportStateChangedResponse transportStateChangedResponse)
    {
        OnTransportStateChangedResponseEvent?.Invoke(transportStateChangedResponse);
    }
}
