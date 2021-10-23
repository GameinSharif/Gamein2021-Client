using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    public GameObject LoginCanvas;
    public GameObject SelectLanguagePopup;

    public TMP_InputField UsernameInputField;
    public TMP_InputField PasswordInputField;
    public Localize LoginErrorLocalize;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoginErrorLocalize.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("Language"))
        {
            LoginCanvas.SetActive(true);
            SelectLanguagePopup.SetActive(false);
        }
        else
        {
            LoginCanvas.SetActive(false);
            SelectLanguagePopup.SetActive(true);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.OnLoginResponseEvent += OnLoginResponseReceive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnLoginResponseEvent -= OnLoginResponseReceive;
    }

    public void SelectLangugae(string language)
    {
        LocalizationManager.Instance.SetLanguage(language);

        LoginCanvas.SetActive(true);
        SelectLanguagePopup.SetActive(false);
    }

    public void OnLoginButtonClick()
    {
        string username = UsernameInputField.text;
        string password = PasswordInputField.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            LoginErrorLocalize.SetKey("login_error_empty");
            LoginErrorLocalize.gameObject.SetActive(true);
            return;
        }

        //string encryptedPassword = EncryptManager.Encrypt(password);
        string encryptedPassword = password;

        LoginRequest loginRequest = new LoginRequest(RequestTypeConstant.LOGIN, username, encryptedPassword);
        RequestManager.Instance.SendRequest(loginRequest);
    }

    public void OnLoginResponseReceive(LoginResponse loginResponse)
    {
        if (loginResponse.result == "Successful")
        {
            int playerId = loginResponse.playerId;
            PlayerPrefs.SetInt("PlayerId" , playerId);
            PlayerPrefs.SetString("Country", loginResponse.team.country.ToString());
            PlayerPrefs.SetInt("TeamId", loginResponse.team.id);
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            LoginErrorLocalize.SetKey("login_error_info");
            LoginErrorLocalize.gameObject.SetActive(true);
        }
    }

    public void print(string s)
    {
        Debug.Log(s);
    }
}
