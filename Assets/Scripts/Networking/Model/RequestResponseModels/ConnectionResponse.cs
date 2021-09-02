using System;

[Serializable]
public class ConnectionResponse : ResponseObject
{
    public byte[] encodedPublicKey;
}