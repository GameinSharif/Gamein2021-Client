using System;

[Serializable]
public class LoginResponse : ResponseObject
{
    public int playerId;
    public string result;
    public Utils.Team team;
    public string token;
}
