using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EncryptManager
{
    public static RSACryptoServiceProvider Rsa;

    public static void OnConnectionResponse(ConnectionResponse connectionResponse)
    {
        var rsaParameters = new RSAParameters();
        rsaParameters.Exponent = connectionResponse.exponent;
        rsaParameters.Modulus = connectionResponse.modules;

        Rsa = new RSACryptoServiceProvider();
        Rsa.ImportParameters(rsaParameters);

        if (PlayerPrefs.HasKey("Language"))
        {
            LoginManager.Instance.GetGameStatus();
        }
    }

    public static string Encrypt(string message)
    {
        var messageBytes = Encoding.Unicode.GetBytes(message);
        var encryptedMessageBytes = Rsa.Encrypt(messageBytes, false);
        var encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);
        return encryptedMessage;
    }
}