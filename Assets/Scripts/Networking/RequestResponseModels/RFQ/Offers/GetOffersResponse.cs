using System;
using System.Collections.Generic;

[Serializable]
public class GetOffersResponse : ResponseObject
{
    public List<Utils.Offer> myTeamOffers;
    public List<Utils.Offer> otherTeamsOffers;
}