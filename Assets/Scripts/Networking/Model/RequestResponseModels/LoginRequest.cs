using System;
using System.Collections.Generic;

[Serializable]
public class LoginRequest
{
    public string Teamname;
    public string Password;

    public LoginRequest(string teamname, string password)
    {
        Teamname = teamname;
        Password = password;
    }
}