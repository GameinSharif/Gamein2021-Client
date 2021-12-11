using System;

[Serializable]
public class ReportMessageRequest : RequestObject
{
    public int chatId;
    public string messageText;
    public int reportedTeamId;
    public CustomDateTime insertedAt;

    public ReportMessageRequest(int chatId, string messageText, int reportedTeamId, CustomDateTime insertedAt) : base(RequestTypeConstant.REPORT_MESSAGE)
    {
        this.chatId = chatId;
        this.insertedAt = insertedAt;
        this.messageText = messageText;
        this.reportedTeamId = reportedTeamId;
    }
}