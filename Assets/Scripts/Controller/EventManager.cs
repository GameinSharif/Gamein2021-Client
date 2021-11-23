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

    public event Action<GetCurrentWeekSuppliesResponse> OnGetCurrentWeekSuppliesResponseEvent;
    public void OnGetCurrentWeekSuppliesResponse(GetCurrentWeekSuppliesResponse getCurrentWeekSuppliesResponse)
    {
        OnGetCurrentWeekSuppliesResponseEvent?.Invoke(getCurrentWeekSuppliesResponse);
    }
    
    public event Action<GetContractSuppliersResponse> OnGetContractSuppliersResponseEvent;
    public void OnGetContractSuppliersResponse(GetContractSuppliersResponse getContractSuppliersResponse)
    {
        OnGetContractSuppliersResponseEvent?.Invoke(getContractSuppliersResponse);
    }
    
    public event Action<NewContractSupplierResponse> OnNewContractSupplierResponseEvent;
    public void OnNewContractSupplierResponse(NewContractSupplierResponse newContractSupplierResponse)
    {
        OnNewContractSupplierResponseEvent?.Invoke(newContractSupplierResponse);
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

    public void OnEditNegotiationCostPerUnitResponse(
        EditNegotiationCostPerUnitResponse editNegotiationCostPerUnitResponse)
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
    
    public event Action<TerminateLongtermContractSupplierResponse> OnTerminateLongtermContractSupplierResponseEvent;
    public void OnTerminateLongtermContractSupplierResponse(TerminateLongtermContractSupplierResponse terminateLongtermContractSupplierResponse)
    {
        OnTerminateLongtermContractSupplierResponseEvent?.Invoke(terminateLongtermContractSupplierResponse);
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

    public event Action<ServerTimeResponse> OnServerTimeResponseEvent;
    public void OnServerTimeResponse(ServerTimeResponse serverTimeResponse)
    {
        OnServerTimeResponseEvent?.Invoke(serverTimeResponse);
    }

    public event Action<GameTimeResponse> OnGameTimeResponseEvent;
    public void OnGameTimeResponse(GameTimeResponse gameTimeResponse)
    {
        OnGameTimeResponseEvent?.Invoke(gameTimeResponse);
    }
    
    public event Action<MoneyUpdateResponse> OnMoneyUpdateResponseEvent;
    public void OnMoneyUpdateResponse(MoneyUpdateResponse moneyUpdateResponse)
    {
        OnMoneyUpdateResponseEvent?.Invoke(moneyUpdateResponse);
    }
    
    public event Action<AcceptOfferResponse> OnAcceptOfferResponseEvent;
    public void OnAcceptOfferResponse(AcceptOfferResponse acceptOfferResponse)
    {
        OnAcceptOfferResponseEvent?.Invoke(acceptOfferResponse);
    }

    public event Action<GetProductionLinesResponse> OnGetProductionLinesResponseEvent;

    public void OnGetProductionLinesResponse(GetProductionLinesResponse response)
    {
        OnGetProductionLinesResponseEvent?.Invoke(response);
    }

    public event Action<ConstructProductionLineResponse> OnConstructProductionLineResponseEvent;

    public void OnConstructProductionLineResponse(ConstructProductionLineResponse response)
    {
        OnConstructProductionLineResponseEvent?.Invoke(response);
    }

    public event Action<ScrapProductionLineResponse> OnScrapProductionLineResponseEvent;

    public void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
    {
        OnScrapProductionLineResponseEvent?.Invoke(response);
    }

    public event Action<StartProductionResponse> OnStartProductionResponseEvent;

    public void OnStartProductionResponse(StartProductionResponse response)
    {
        OnStartProductionResponseEvent?.Invoke(response);
    }

    public event Action<UpgradeProductionLineEfficiencyResponse> OnUpgradeProductionLineEfficiencyResponseEvent;

    public void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
    {
        OnUpgradeProductionLineEfficiencyResponseEvent?.Invoke(response);
    }

    public event Action<UpgradeProductionLineQualityResponse> OnUpgradeProductionLineQualityResponseEvent;

    public void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
    {
        OnUpgradeProductionLineQualityResponseEvent?.Invoke(response);
    }
    
    public event Action<GetAllActiveDcResponse> OnGetAllActiveDcResponseEvent;

    public void OnGetAllActiveDcResponse(GetAllActiveDcResponse response)
    {
        OnGetAllActiveDcResponseEvent?.Invoke(response);
    }

    public event Action<RemoveProductResponse> OnRemoveProductResponseEvent;

    public void OnRemoveProductResponse(RemoveProductResponse response)
    {
        OnRemoveProductResponseEvent?.Invoke(response);
    }

    public event Action<StartTransportForPlayerStoragesResponse> OnStartTransportForPlayerStoragesResponseEvent;
    public void OnStartTransportForPlayerStoragesResponse(StartTransportForPlayerStoragesResponse response)
    {
        OnStartTransportForPlayerStoragesResponseEvent?.Invoke(response);
    }
}
