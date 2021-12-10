using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatData
{
    public CustomDateTime latestMessageDate;
    public int team1Id;
    public int team2Id;
    public List<MessageData> messages;

    public string TeamName
    {
        get
        {
            return GameDataManager.Instance.GetTeamName(TheirTeamId);
        }
    }

    public int TheirTeamId
    {
        get
        {
            return PlayerPrefs.GetInt("TeamId") == team1Id ? team2Id : team1Id;
        }
    }
}