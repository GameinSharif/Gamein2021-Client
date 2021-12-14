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
    public GameObject usernameError;
    public GameObject passwordError;

    private string _username;
    private string _password;
    
    private void Awake()
    {
        Instance = this;

        Screen.fullScreen = false;
    }

    private void Start()
    {
        LoginErrorLocalize.gameObject.SetActive(false);
        passwordError.SetActive(false);
        usernameError.SetActive(false);

        if (PlayerPrefs.HasKey("Language"))
        {
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
        SelectLanguagePopup.SetActive(false);
        GetGameStatus();
    }

    private bool HasDataForLogin()
    {
        return PlayerPrefs.HasKey("Username") && PlayerPrefs.HasKey("Password");
    }

    public void GetGameStatus()
    {
        GetGameStatusRequest getGameStatusRequest = new GetGameStatusRequest(RequestTypeConstant.GET_GAME_STATUS);
        RequestManager.Instance.SendRequest(getGameStatusRequest);

        if (HasDataForLogin())
        {
            SendLoginRequest(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password"));
        }
        else
        {
            OpenLoginPopup();
        }
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

        SendLoginRequest(username, password);
    }

    private void SendLoginRequest(string username, string password)
    {
        _username = username;
        _password = password;
        string encryptedPassword = EncryptManager.Encrypt(password);

        LoginRequest loginRequest = new LoginRequest(RequestTypeConstant.LOGIN, username, encryptedPassword);
        RequestManager.Instance.SendRequest(loginRequest);
    }

    public void OnLoginResponseReceive(LoginResponse loginResponse)
    {
        if (loginResponse.result == "Successful")
        {
            PlayerPrefs.SetInt("PlayerId" , loginResponse.playerId);
            PlayerPrefs.SetString("TeamName", loginResponse.team.teamName);
            PlayerPrefs.SetInt("TeamId", loginResponse.team.id);
            PlayerPrefs.SetInt("FactoryId", loginResponse.team.factoryId); //Is 0 if player has no factory
            PlayerPrefs.SetString("Country", loginResponse.team.country.ToString());
            PlayerPrefs.SetFloat("Money", loginResponse.team.credit);
            PlayerPrefs.SetFloat("Value", loginResponse.team.wealth);
            PlayerPrefs.SetFloat("Brand", loginResponse.team.brand);
            PlayerPrefs.SetFloat("DonatedAmount", loginResponse.team.donatedAmount);

            PlayerPrefs.SetString("Username", _username);
            PlayerPrefs.SetString("Password", _password);

            RequestObject.myToken = loginResponse.token;

            SceneManager.LoadScene("MenuScene");
        }
        else if (loginResponse.result == "Can not login. Already logged in.")
        {
            LoginErrorLocalize.SetKey("login_already_logged_in_error");
            LoginErrorLocalize.gameObject.SetActive(true);
            OpenLoginPopup();
        }
        else
        {
            LoginErrorLocalize.SetKey("login_error_info");
            LoginErrorLocalize.gameObject.SetActive(true);
            passwordError.SetActive(true);
            usernameError.SetActive(true);
            OpenLoginPopup();
        }
    }

    private void OpenLoginPopup()
    {
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        LoginCanvas.SetActive(true);
    }
}
