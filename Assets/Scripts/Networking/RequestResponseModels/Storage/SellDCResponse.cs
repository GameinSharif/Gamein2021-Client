using System;

[Serializable]
public class SellDCResponse : ResponseObject
{
    public Utils.DC dcDto;
    public string result;
}