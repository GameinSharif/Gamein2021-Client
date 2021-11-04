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
            case ResponseTypeConstant.GET_CURRENT_WEEK_SUPPLIES:
                GetCurrentWeekSuppliesResponse getCurrentWeekSuppliesResponse = JsonUtility.FromJson<GetCurrentWeekSuppliesResponse>(responseJson);
                EventManager.Instance.OnGetCurrentWeekSuppliesResponse(getCurrentWeekSuppliesResponse);
                break;
            case ResponseTypeConstant.GET_CONTRACT_SUPPLIERS:
                GetContractSuppliersResponse getContractSuppliersResponse = JsonUtility.FromJson<GetContractSuppliersResponse>(responseJson);
                EventManager.Instance.OnGetContractSuppliersResponse(getContractSuppliersResponse);
                break;
            case ResponseTypeConstant.GET_CONTRACTS:
                GetContractsResponse getContractsResponse = JsonUtility.FromJson<GetContractsResponse>(responseJson);
                EventManager.Instance.OnGetContractsResponse(getContractsResponse);
                break;
            case ResponseTypeConstant.NEW_NEGOTIATION:
                //TODO
                break;
            case ResponseTypeConstant.GET_NEGOTIATIONS:
                //TODO
                break;
            case ResponseTypeConstant.EDIT_NEGOTIATION_COST_PER_UNIT:
                //TODO
                break;
            case ResponseTypeConstant.NEW_PROVIDER:
                NewProviderResponse newProviderResponse = JsonUtility.FromJson<NewProviderResponse>(responseJson);
                EventManager.Instance.OnNewProviderResponse(newProviderResponse);
                break;
            case ResponseTypeConstant.GET_PROVIDERS:
                GetProvidersResponse getProvidersResponse = JsonUtility.FromJson<GetProvidersResponse>(responseJson);
                EventManager.Instance.OnGetProvidersResponse(getProvidersResponse);
                break;
            case ResponseTypeConstant.REMOVE_PROVIDER:
                //TODO
                break;
            case ResponseTypeConstant.NEW_PROVIDER_NEGOTIATION:
                //TODO
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
                //TODO
                break;
            case ResponseTypeConstant.NEW_MESSAGE:
                NewMessageResponse newMessageResponse = JsonUtility.FromJson<NewMessageResponse>(responseJson);
                EventManager.Instance.OnNewMessageResponse(newMessageResponse);
                break;
            case ResponseTypeConstant.GET_All_CHATS:
                GetAllChatsResponse getAllChatsResponse = JsonUtility.FromJson<GetAllChatsResponse>(responseJson);
                EventManager.Instance.OnGetAllChatsResponse(getAllChatsResponse);
                break;
        }
    }
}
