using System;

[Serializable]
public class ConnectionResponse : ResponseObject
{
    public byte[] exponent;
    public byte[] modules;
}