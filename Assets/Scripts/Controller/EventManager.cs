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

    public event Action<LoginResponse> OnLoginResponseEvent;  
    public void OnLoginResponse(LoginResponse loginResponse)
    {
        OnLoginResponseEvent?.Invoke(loginResponse);
    }
}
