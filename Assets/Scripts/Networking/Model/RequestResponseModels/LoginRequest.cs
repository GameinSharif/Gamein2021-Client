using System;

[Serializable]
public class LoginRequest : RequestObject
{
    public string username;
    public string password;

    public LoginRequest(RequestTypeConstant requestTypeConstant, string username, string password) : base(requestTypeConstant)
    {
        this.username = username;
        this.password = password;
    }
}