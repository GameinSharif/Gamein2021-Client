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
                LoginResponse loginResponseObject = JsonUtility.FromJson<LoginResponse>(responseJson);
                EventManager.Instance.OnLoginResponse(loginResponseObject);
                break;
            case ResponseTypeConstant.NEW_OFFER:
                NewOfferResponse newOfferResponse = JsonUtility.FromJson<NewOfferResponse>(responseJson);
                EventManager.Instance.OnNewOfferResponse(newOfferResponse);
                break;
            case ResponseTypeConstant.GET_OFFERS:
                GetOffersResponse getOffersResponse = JsonUtility.FromJson<GetOffersResponse>(responseJson);
                EventManager.Instance.OnGetOffersResponse(getOffersResponse);
                break;
            case ResponseTypeConstant.GET_GAME_DATA:
                GetGameDataResponse getGameDataResponse = JsonUtility.FromJson<GetGameDataResponse>(responseJson);
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
                //TODO
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
                GetAllAuctionsResponse getAllAuctionsResponse = JsonUtility.FromJson<GetAllAuctionsResponse>(responseJson);
                EventManager.Instance.OnGetAllAuctionsResponse(getAllAuctionsResponse);
                break;
            case ResponseTypeConstant.BID_FOR_AUCTION:
                BidForAuctionResponse bidForAuctionResponse = JsonUtility.FromJson<BidForAuctionResponse>(responseJson);
                EventManager.Instance.OnBidForAuctionResponse(bidForAuctionResponse);
                break;
        }
    }
}
