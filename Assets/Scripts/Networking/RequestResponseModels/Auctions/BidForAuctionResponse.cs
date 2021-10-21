using System;

[Serializable]
public class BidForAuctionResponse : ResponseObject
{
    public string result;
    public int highestBidAmount;
    public int bidderPlayerId;
    public MapUtils.MapAgentMarker.AgentType agentType;
}