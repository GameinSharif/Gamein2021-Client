using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class RequestHandler
{
    public static void HandleServerResponse(string responseJson)
    {
        ResponseObject responseObject = JsonUtility.FromJson<ResponseObject>(responseJson);
        ResponseTypeConstant responseTypeConstant = (ResponseTypeConstant)responseObject.responseTypeConstant;
        switch (responseTypeConstant)
        {
            case ResponseTypeConstant.CONNECTION:
                ConnectionResponse connectionResponse = JsonUtility.FromJson<ConnectionResponse>(responseJson);
                EventManager.Instance.OnConnectionResponse(connectionResponse);
                break;
            case ResponseTypeConstant.LOGIN:
                LoginResponse loginResponse = JsonConvert.DeserializeObject(responseJson, typeof(LoginResponse), new StringEnumConverter()) as LoginResponse;
                EventManager.Instance.OnLoginResponse(loginResponse);
                break;
            case ResponseTypeConstant.NEW_OFFER:
                NewOfferResponse newOfferResponse = JsonUtility.FromJson<NewOfferResponse>(responseJson);
                EventManager.Instance.OnNewOfferResponse(newOfferResponse);
                break;
            case ResponseTypeConstant.GET_OFFERS:
                GetOffersResponse getOffersResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetOffersResponse), new StringEnumConverter()) as GetOffersResponse;
                EventManager.Instance.OnGetOffersResponse(getOffersResponse);
                break;
            case ResponseTypeConstant.GET_GAME_DATA:
                GetGameDataResponse getGameDataResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetGameDataResponse), new StringEnumConverter()) as GetGameDataResponse;
                EventManager.Instance.OnGetGameDataResponse(getGameDataResponse);
                break;
            case ResponseTypeConstant.GET_CURRENT_WEEK_DEMANDS:
                GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse = JsonUtility.FromJson<GetCurrentWeekDemandsResponse>(responseJson);
                EventManager.Instance.OnGetCurrentWeekDemandsResponse(getCurrentWeekDemandsResponse);
                break;
            case ResponseTypeConstant.GET_CONTRACTS:
                GetContractsResponse getContractsResponse = JsonUtility.FromJson<GetContractsResponse>(responseJson);
                EventManager.Instance.OnGetContractsResponse(getContractsResponse);
                break;
            case ResponseTypeConstant.ACCEPT_OFFER:
                //TODO
                break;
            case ResponseTypeConstant.GET_NEGOTIATIONS:
                GetNegotiationsResponse getNegotiationsResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetNegotiationsResponse), new StringEnumConverter()) as GetNegotiationsResponse;
                EventManager.Instance.OnGetNegotiationsResponse(getNegotiationsResponse);
                break;
            case ResponseTypeConstant.EDIT_NEGOTIATION_COST_PER_UNIT:
                EditNegotiationCostPerUnitResponse editNegotiationCostPerUnitResponse = JsonConvert.DeserializeObject(responseJson, typeof(EditNegotiationCostPerUnitResponse), new StringEnumConverter()) as EditNegotiationCostPerUnitResponse;
                EventManager.Instance.OnEditNegotiationCostPerUnitResponse(editNegotiationCostPerUnitResponse);
                break;
            case ResponseTypeConstant.NEW_PROVIDER:
                NewProviderResponse newProviderResponse = JsonUtility.FromJson<NewProviderResponse>(responseJson);
                EventManager.Instance.OnNewProviderResponse(newProviderResponse);
                break;
            case ResponseTypeConstant.GET_PROVIDERS:
                GetProvidersResponse getProvidersResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetProvidersResponse), new StringEnumConverter()) as GetProvidersResponse;
                EventManager.Instance.OnGetProvidersResponse(getProvidersResponse);
                break;
            case ResponseTypeConstant.REMOVE_PROVIDER:
                RemoveProviderResponse removeProviderResponse = JsonUtility.FromJson<RemoveProviderResponse>(responseJson);
                EventManager.Instance.OnRemoveProviderResponse(removeProviderResponse);
                break;
            case ResponseTypeConstant.NEW_PROVIDER_NEGOTIATION:
                NewProviderNegotiationResponse newProviderNegotiationResponse = JsonConvert.DeserializeObject(responseJson, typeof(NewProviderNegotiationResponse), new StringEnumConverter()) as NewProviderNegotiationResponse;
                EventManager.Instance.OnNewProviderNegotiationResponse(newProviderNegotiationResponse);
                break;
            case ResponseTypeConstant.GET_ALL_AUCTIONS:
                GetAllAuctionsResponse getAllAuctionsResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetAllAuctionsResponse), new StringEnumConverter()) as GetAllAuctionsResponse;
                EventManager.Instance.OnGetAllAuctionsResponse(getAllAuctionsResponse);
                break;
            case ResponseTypeConstant.BID_FOR_AUCTION:
                BidForAuctionResponse bidForAuctionResponse = JsonConvert.DeserializeObject(responseJson, typeof(BidForAuctionResponse), new StringEnumConverter()) as BidForAuctionResponse;
                EventManager.Instance.OnBidForAuctionResponse(bidForAuctionResponse);
                break;
            case ResponseTypeConstant.TERMINATE_OFFER:
                TerminateOfferResponse terminateOfferResponse = JsonUtility.FromJson<TerminateOfferResponse>(responseJson);
                EventManager.Instance.OnTerminateOfferResponse(terminateOfferResponse);
                break;
            case ResponseTypeConstant.NEW_MESSAGE:
                NewMessageResponse newMessageResponse = JsonUtility.FromJson<NewMessageResponse>(responseJson);
                EventManager.Instance.OnNewMessageResponse(newMessageResponse);
                break;
            case ResponseTypeConstant.GET_ALL_CHATS:
                GetAllChatsResponse getAllChatsResponse = JsonUtility.FromJson<GetAllChatsResponse>(responseJson);
                EventManager.Instance.OnGetAllChatsResponse(getAllChatsResponse);
                break;
            case ResponseTypeConstant.GET_TEAM_TRANSPORTS:
                GetTeamTransportsResponse getTeamTransportsResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetTeamTransportsResponse), new StringEnumConverter()) as GetTeamTransportsResponse;
                EventManager.Instance.OnGetTeamTransportsResponse(getTeamTransportsResponse);
                break;
            case ResponseTypeConstant.TRANSPORT_STATE_CHANGED:
                TransportStateChangedResponse transportStateChangedResponse = JsonConvert.DeserializeObject(responseJson, typeof(TransportStateChangedResponse), new StringEnumConverter()) as TransportStateChangedResponse;
                EventManager.Instance.OnTransportStateChangedResponse(transportStateChangedResponse);
                break;
            case ResponseTypeConstant.AUCTION_FINISHED:
                AuctionFinishedResponse auctionFinishedResponse = JsonConvert.DeserializeObject(responseJson, typeof(AuctionFinishedResponse), new StringEnumConverter()) as AuctionFinishedResponse;
                EventManager.Instance.OnAuctionFinishedResponse(auctionFinishedResponse);
                break;
        }
    }
}
