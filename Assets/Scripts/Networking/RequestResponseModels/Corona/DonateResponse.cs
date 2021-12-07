using System;
using System.Collections.Generic;

[Serializable]
public class DonateResponse : ResponseObject
{
    public List<Utils.CoronaInfoDto> infos;
    public string result;
}