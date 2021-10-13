using System;

[Serializable]
public class ChangeMapMarkerResponse : ResponseObject
{
    public string result;
    public int highestBidAmount;
    public int bidderPlayerId;
    public MapUtils.MapAgentMarker.AgentType agentType;
}