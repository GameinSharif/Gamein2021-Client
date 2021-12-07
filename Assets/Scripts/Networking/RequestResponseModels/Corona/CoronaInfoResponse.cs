using System;
using System.Collections.Generic;

[Serializable]
public class CoronaInfoResponse : ResponseObject
{
    public List<Utils.CoronaInfoDto> infos;
}