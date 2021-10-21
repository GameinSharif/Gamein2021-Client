using System;

[Serializable]
public class LoginResponse : ResponseObject
{
    public int playerId;
    public string result;
    public bool isFirstTime;
    public string teamName;
    public int factoryId;
    public string country;
}