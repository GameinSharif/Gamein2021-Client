using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnectManager : MonoBehaviour
{
    public static ReconnectManager Instance;

    public GameObject ReconnectPopup;
    public GameObject WaitForReconnectText;
    public GameObject ReconnectFailedText;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void OpenReconnectPopup()
    {
        ReconnectPopup.SetActive(true);
        WaitForReconnectText.SetActive(true);
        ReconnectFailedText.SetActive(false);
    }

    public void OnReconnect()
    {
        ReconnectPopup.SetActive(false);
    }

    public void OnReconnectFail()
    {
        ReconnectPopup.SetActive(true);
        WaitForReconnectText.SetActive(false);
        ReconnectFailedText.SetActive(true);
    }
}
