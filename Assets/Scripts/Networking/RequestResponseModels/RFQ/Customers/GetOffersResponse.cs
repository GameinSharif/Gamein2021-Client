using System;
using System.Collections.Generic;

[Serializable]
public class GetOffersResponse : ResponseObject
{
    public List<GetOffersTransitModel> offers;
}