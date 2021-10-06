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

    public event Action<GetOffersResponse> OnGetOffersResponseEvent;
    public void OnGetOffersResponse(GetOffersResponse getOffersResponse)
    {
        OnGetOffersResponseEvent?.Invoke(getOffersResponse);
    }

    public event Action<NewOfferResponse> OnNewOfferResponseEvent;
    public void OnNewOfferResponse(NewOfferResponse newOfferResponse)
    {
        OnNewOfferResponseEvent?.Invoke(newOfferResponse);
    }

    public event Action<GetContractsResponse> OnGetContractsResponseEvent;
    public void OnGetContractsResponse(GetContractsResponse getContractsResponse)
    {
        OnGetContractsResponseEvent?.Invoke(getContractsResponse);
    }

    public event Action<GetGameDataResponse> OnGetGameDataResponseEvent;
    public void OnGetGameDataResponse(GetGameDataResponse getGameDataResponse)
    {
        OnGetGameDataResponseEvent?.Invoke(getGameDataResponse);
    }
}
