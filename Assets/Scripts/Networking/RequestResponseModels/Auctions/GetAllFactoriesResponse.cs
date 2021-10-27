using System.Collections.Generic;
using System;

[Serializable]
public class GetAllFactoriesResponse : ResponseObject
{
    public List<Utils.Factory> factories;
}