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
                GetCurrentWeekDemandsResponse getCurrentWeekDemandsResponse =
                    JsonUtility.FromJson<GetCurrentWeekDemandsResponse>(responseJson);
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
            case ResponseTypeConstant.GET_ALL_AUCTIONS:
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}