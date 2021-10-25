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
}
