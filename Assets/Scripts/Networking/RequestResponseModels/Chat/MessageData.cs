using System;
using UnityEngine;

[Serializable]
public class MessageData
{
    public int senderTeamId;
    public int receiverTeamId;
    public string text;
    public CustomDateTime insertedAt;

    public bool IsFromMe
    {
        get
        {
            return PlayerPrefs.GetInt("TeamId") == senderTeamId;
        }

        set
        {
            IsFromMe = value;
        }
    }

    public int TheirTeamId => IsFromMe ? receiverTeamId : senderTeamId;
}