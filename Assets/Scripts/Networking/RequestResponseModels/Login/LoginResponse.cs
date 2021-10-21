using System;

[Serializable]
public class LoginResponse : ResponseObject
{
    public int playerId;
    public string result;
    private Utils.Team team;
}
