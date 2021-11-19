using System;

[Serializable]
public class NewMessageRequest : RequestObject
{
    public int receiverTeamId;
    public string text;

    public NewMessageRequest(int receiverTeamId, string text) : base(RequestTypeConstant.NEW_MESSAGE)
    {
        this.receiverTeamId = receiverTeamId;
        this.text = text;
    }
}