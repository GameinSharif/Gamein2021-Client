using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class RequestHandler
{
    public static void HandleServerResponse(string responseJson)
    {
        ResponseObject responseObject = JsonUtility.FromJson<ResponseObject>(responseJson);
        ResponseTypeConstant responseTypeConstant = (ResponseTypeConstant) responseObject.responseTypeConstant;
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
                GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse =
                    JsonUtility.FromJson<GetCurrentWeekDemandsResponse>(responseJson);
                EventManager.Instance.OnGetCurrentWeekDemandsResponse(getCurrentWeekDemandsResponse);
                break;
            case ResponseTypeConstant.GET_CURRENT_WEEK_SUPPLIES:
                GetCurrentWeekSuppliesResponse getCurrentWeekSuppliesResponse = JsonUtility.FromJson<GetCurrentWeekSuppliesResponse>(responseJson);
                EventManager.Instance.OnGetCurrentWeekSuppliesResponse(getCurrentWeekSuppliesResponse);
                break;
            case ResponseTypeConstant.GET_CONTRACTS_WITH_SUPPLIER:
                GetContractSuppliersResponse getContractSuppliersResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetContractSuppliersResponse), new StringEnumConverter()) as GetContractSuppliersResponse;
                EventManager.Instance.OnGetContractSuppliersResponse(getContractSuppliersResponse);
                break;
            case ResponseTypeConstant.NEW_CONTRACT_WITH_SUPPLIER:
                NewContractSupplierResponse newContractSupplierResponse = JsonConvert.DeserializeObject(responseJson, typeof(NewContractSupplierResponse), new StringEnumConverter()) as NewContractSupplierResponse;
                EventManager.Instance.OnNewContractSupplierResponse(newContractSupplierResponse);
                break;
            case ResponseTypeConstant.GET_CONTRACTS:
                GetContractsResponse getContractsResponse = JsonUtility.FromJson<GetContractsResponse>(responseJson);
                EventManager.Instance.OnGetContractsResponse(getContractsResponse);
                break;
            case ResponseTypeConstant.ACCEPT_OFFER:
                AcceptOfferResponse acceptOfferResponse = JsonUtility.FromJson<AcceptOfferResponse>(responseJson);
                EventManager.Instance.OnAcceptOfferResponse(acceptOfferResponse);
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
            case ResponseTypeConstant.BUY_DC:
                BuyDCResponse buyDcResponse = JsonConvert.DeserializeObject(responseJson, typeof(BuyDCResponse), new StringEnumConverter()) as BuyDCResponse;
                EventManager.Instance.OnBuyDCResponse(buyDcResponse);
                break;
            case ResponseTypeConstant.SELL_DC:
                SellDCResponse sellDcResponse = JsonConvert.DeserializeObject(responseJson, typeof(SellDCResponse), new StringEnumConverter()) as SellDCResponse;
                EventManager.Instance.OnSellDCResponse(sellDcResponse);
                break;
            case ResponseTypeConstant.GET_STORAGES:
                GetStorageProductsResponse getStorageProductsResponse =
                    JsonUtility.FromJson<GetStorageProductsResponse>(responseJson);
                EventManager.Instance.OnGetStorageProductsResponse(getStorageProductsResponse);
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
            case ResponseTypeConstant.TERMINATE_LONGTERM_CONTRACT_WITH_SUPPLIER:
                TerminateLongtermContractSupplierResponse terminateLongtermContractSupplierResponse = JsonUtility.FromJson<TerminateLongtermContractSupplierResponse>(responseJson);
                EventManager.Instance.OnTerminateLongtermContractSupplierResponse(terminateLongtermContractSupplierResponse);
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
            case ResponseTypeConstant.GET_ALL_ACTIVE_DC:
                GetAllActiveDcResponse getAllActiveDcResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetAllActiveDcResponse), new StringEnumConverter()) as GetAllActiveDcResponse;
                EventManager.Instance.OnGetAllActiveDcResponse(getAllActiveDcResponse);
                break;
            case ResponseTypeConstant.GET_PRODUCTION_LINES:
                EventManager.Instance.OnGetProductionLinesResponse(
                    JsonConvert.DeserializeObject(responseJson, typeof(GetProductionLinesResponse),
                        new StringEnumConverter()) as GetProductionLinesResponse);
                break;
            case ResponseTypeConstant.CONSTRUCT_PRODUCTION_LINE:
                EventManager.Instance.OnConstructProductionLineResponse(JsonConvert.DeserializeObject(responseJson,
                    typeof(ConstructProductionLineResponse),
                    new StringEnumConverter()) as ConstructProductionLineResponse);
                break;
            case ResponseTypeConstant.SCRAP_PRODUCTION_LINE:
                EventManager.Instance.OnScrapProductionLineResponse(JsonConvert.DeserializeObject(responseJson,
                    typeof(ScrapProductionLineResponse),
                    new StringEnumConverter()) as ScrapProductionLineResponse);
                break;
            case ResponseTypeConstant.START_PRODUCTION:
                EventManager.Instance.OnStartProductionResponse(JsonConvert.DeserializeObject(responseJson,
                    typeof(StartProductionResponse),
                    new StringEnumConverter()) as StartProductionResponse);
                break;
            case ResponseTypeConstant.UPGRADE_PRODUCTION_LINE_QUALITY:
                EventManager.Instance.OnUpgradeProductionLineQualityResponse(JsonConvert.DeserializeObject(responseJson,
                    typeof(UpgradeProductionLineQualityResponse),
                    new StringEnumConverter()) as UpgradeProductionLineQualityResponse);
                break;
            case ResponseTypeConstant.UPGRADE_PRODUCTION_LINE_EFFICIENCY:
                EventManager.Instance.OnUpgradeProductionLineEfficiencyResponse(JsonConvert.DeserializeObject(
                    responseJson, typeof(UpgradeProductionLineEfficiencyResponse),
                    new StringEnumConverter()) as UpgradeProductionLineEfficiencyResponse);
                break;
            case ResponseTypeConstant.PRODUCTION_LINE_CONSTRUCTION_COMPLETED:
                EventManager.Instance.OnProductionLineConstructionCompletedResponse(JsonConvert.DeserializeObject(
                    responseJson, typeof(ProductionLineConstructionCompletedResponse),
                    new StringEnumConverter()) as ProductionLineConstructionCompletedResponse);
                break;
            case ResponseTypeConstant.PRODUCT_CREATION_COMPLETED:
                EventManager.Instance.OnProductCreationCompletedResponse(JsonConvert.DeserializeObject(
                    responseJson, typeof(ProductCreationCompletedResponse),
                    new StringEnumConverter()) as ProductCreationCompletedResponse);
                break;
            case ResponseTypeConstant.SERVER_TIME:
                ServerTimeResponse serverTimeResponse = JsonUtility.FromJson<ServerTimeResponse>(responseJson);
                EventManager.Instance.OnServerTimeResponse(serverTimeResponse);
                break;
            case ResponseTypeConstant.GAME_TIME:
                GameTimeResponse gameTimeResponse = JsonUtility.FromJson<GameTimeResponse>(responseJson);
                EventManager.Instance.OnGameTimeResponse(gameTimeResponse);
                break;
            case ResponseTypeConstant.MONEY_UPDATE:
                MoneyUpdateResponse moneyUpdateResponse = JsonUtility.FromJson<MoneyUpdateResponse>(responseJson);
                EventManager.Instance.OnMoneyUpdateResponse(moneyUpdateResponse);
                break;
            case ResponseTypeConstant.REMOVE_PRODUCT:
                RemoveProductResponse removeProductResponse = JsonConvert.DeserializeObject(responseJson, typeof(RemoveProductResponse), new StringEnumConverter()) as RemoveProductResponse;
                EventManager.Instance.OnRemoveProductResponse(removeProductResponse);
                break;
            case ResponseTypeConstant.NEW_CONTRACT:
                NewContractResponse newContractResponse = JsonConvert.DeserializeObject(responseJson, typeof(NewContractResponse), new StringEnumConverter()) as NewContractResponse;
                EventManager.Instance.OnNewContractResponse(newContractResponse);
                break;
            case ResponseTypeConstant.TERMINATE_CONTRACT:
                TerminateLongtermContractResponse terminateLongtermContractResponse = JsonConvert.DeserializeObject(responseJson, typeof(TerminateLongtermContractResponse), new StringEnumConverter()) as TerminateLongtermContractResponse;
                EventManager.Instance.OnTerminateLongtermContractResponse(terminateLongtermContractResponse);
                break;
            case ResponseTypeConstant.TRANSPORT_TO_STORAGE:
                StartTransportForPlayerStoragesResponse startTransportForPlayerStoragesResponse =
                    JsonUtility.FromJson<StartTransportForPlayerStoragesResponse>(responseJson);
                EventManager.Instance.OnStartTransportForPlayerStoragesResponse(startTransportForPlayerStoragesResponse);
                break;
            case ResponseTypeConstant.REJECT_NEGOTIATION:
                RejectNegotiationResponse rejectNegotiationResponse =
                    JsonUtility.FromJson<RejectNegotiationResponse>(responseJson);
                EventManager.Instance.OnRejectNegotiationResponse(rejectNegotiationResponse);
                break;
            case ResponseTypeConstant.EDIT_PROVIDER:
                EditProviderResponse editProviderResponse = JsonUtility.FromJson<EditProviderResponse>(responseJson);
                EventManager.Instance.OnEditProviderResponse(editProviderResponse);
            case ResponseTypeConstant.ADD_PRODUCT:
                //TODO remove this from server and here too
                break;
            case ResponseTypeConstant.UPDATE_GAME_STATUS:
                UpdateGameStatusResponse updateGameStatusResponse = JsonConvert.DeserializeObject(responseJson, typeof(UpdateGameStatusResponse), new StringEnumConverter()) as UpdateGameStatusResponse;
                EventManager.Instance.OnUpdateGameStatusResponse(updateGameStatusResponse);
                break;
            case ResponseTypeConstant.ACCESS_DENIED:
                //TODO
                break;
            case ResponseTypeConstant.GET_ALL_WEEKLY_REPORTS:
                GetAllWeeklyReportsResponse getAllWeeklyReportsResponse = JsonConvert.DeserializeObject(responseJson, typeof(GetAllWeeklyReportsResponse), new StringEnumConverter()) as GetAllWeeklyReportsResponse;
                EventManager.Instance.OnGetAllWeeklyReportsResponse(getAllWeeklyReportsResponse);
                break;
            case ResponseTypeConstant.UPDATE_WEEKLY_REPORT:
                UpdateWeeklyReportResponse updateWeeklyReportResponse = JsonConvert.DeserializeObject(responseJson, typeof(UpdateWeeklyReportResponse), new StringEnumConverter()) as UpdateWeeklyReportResponse;
                EventManager.Instance.OnUpdateWeeklyReportResponse(updateWeeklyReportResponse);
                break;
            default:
                Debug.LogWarning(responseJson);
                break;
        }
    }
}
