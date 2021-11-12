using System.Collections.Generic;
using System;

[Serializable]
public class AuctionFinishedResponse : ResponseObject
{
    public List<Utils.Team> teams;
}
