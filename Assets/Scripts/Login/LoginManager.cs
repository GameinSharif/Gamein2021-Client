using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Localize loginError;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        loginError.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnLoginResponseEvent += OnLoginResponseReceive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnLoginResponseEvent -= OnLoginResponseReceive;
    }

    public void OnLoginButtonClick()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            loginError.SetKey("login_error_empty");
            loginError.gameObject.SetActive(true);
            return;
        }

        LoginRequest loginRequest = new LoginRequest(RequestTypeConstant.LOGIN, username, password);
        RequestManager.Instance.SendRequest(loginRequest);
    }

    public void OnLoginResponseReceive(LoginResponse loginResponse)
    {
        if (loginResponse.result == "Successful")
        {
            int playerId = loginResponse.playerId;
            PlayerPrefs.SetInt("PlayerId" , playerId);
            
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            loginError.SetKey("login_error_info");
            loginError.gameObject.SetActive(true);
        }
    }

    public void print(string s)
    {
        Debug.Log(s);
    }
}
