using System;
using System.Security.Cryptography;
using System.Text;

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

        GetGameStatusRequest getGameStatusRequest = new GetGameStatusRequest(RequestTypeConstant.GET_GAME_STATUS);
        RequestManager.Instance.SendRequest(getGameStatusRequest);
    }

    public static string Encrypt(string message)
    {
        var messageBytes = Encoding.ASCII.GetBytes(message);
        var encryptedMessageBytes = Rsa.Encrypt(messageBytes, false);
        var encryptedMessage = Encoding.ASCII.GetString(encryptedMessageBytes);
        return encryptedMessage;
    }
}