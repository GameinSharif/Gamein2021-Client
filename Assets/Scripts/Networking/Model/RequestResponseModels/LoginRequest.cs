using System;
using UnityEngine;

[Serializable]
public class LoginRequest : RequestObject
{
    public string username;
    public string password;

    public LoginRequest(RequestTypeConstant requestTypeConstant, string username, string password)
    {
        this.playerId = PlayerPrefs.GetInt("PlayerId", 0);
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
        this.username = username;
        this.password = password;
    }
}