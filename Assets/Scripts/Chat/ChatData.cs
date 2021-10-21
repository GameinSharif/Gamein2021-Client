using System;
using System.Collections.Generic;

[Serializable]
public class ChatData
{
    
    public int id;
    public CustomDateTime latestMessageDate;
    public int team1Id;
    public int team2Id;
    public List<MessageData> messages;

    public string TeamName
    {
        //TODO get team name from somewhere
        get;
        set;
    }

    //TODO check team1Id and team2Id with our teamId and return theirs
    public int TheirTeamId => team2Id;
}