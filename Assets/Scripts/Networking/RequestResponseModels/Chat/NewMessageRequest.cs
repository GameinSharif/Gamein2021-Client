using System;

[Serializable]
public class NewMessageRequest : RequestObject
{
    public int receiverTeamId;
    public string text;
    public CustomDateTime insertedAt;

    public NewMessageRequest(int receiverTeamId, string text, CustomDateTime insertedAt) : base(RequestTypeConstant.NEW_MESSAGE)
    {
        this.receiverTeamId = receiverTeamId;
        this.text = text;
        this.insertedAt = insertedAt;
    }
}