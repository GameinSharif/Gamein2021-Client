using System;

[Serializable]
public class BuyDCResponse : ResponseObject
{
    public Utils.DC dcDto;
    public string result;
}