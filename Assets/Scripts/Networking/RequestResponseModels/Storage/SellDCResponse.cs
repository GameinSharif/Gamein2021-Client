using System;

[Serializable]
public class SellDCResponse : ResponseObject
{
    public Utils.DCDto dcDto;
    public string result;
}