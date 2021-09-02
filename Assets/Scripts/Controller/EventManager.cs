﻿using System;
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
}
