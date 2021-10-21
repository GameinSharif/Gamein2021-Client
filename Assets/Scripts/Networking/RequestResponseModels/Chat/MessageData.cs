using System;

[Serializable]
public class MessageData
{
    public int id;
    public int senderTeamId;
    public int receiverTeamId;
    public string text;
    public CustomDateTime insertedAt;

    public bool IsFromMe
    {
        //TODO check for teamId and remove set
        get;
        set;
    }

    public int TheirTeamId => IsFromMe ? receiverTeamId : senderTeamId;
}