using System;

[Serializable]
public class BuyDCResponse : ResponseObject
{
    public Utils.DCDto dcDto;
    public string result;
}