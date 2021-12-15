using System;
using UnityEngine;

[Serializable]
public class MessageData
{
    public int id;
    public int chatId;
    public int senderTeamId;
    public int receiverTeamId;
    public string text;
    public CustomDateTime insertedAt;

    public bool IsFromMe => PlayerPrefs.GetInt("TeamId") == senderTeamId;

    public int TheirTeamId => IsFromMe ? receiverTeamId : senderTeamId;
}