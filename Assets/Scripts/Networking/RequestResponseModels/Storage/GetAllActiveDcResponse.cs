using System;
using System.Collections.Generic;

[Serializable]
public class GetAllActiveDcResponse : ResponseObject
{
    public List<Utils.DC> dcs;
}