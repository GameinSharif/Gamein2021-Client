using System.Collections.Generic;

public class ChatData
{
    public ChatData(int chatId, string teamName, List<MessageData> messagas)
    {
        ChatId = chatId;
        TeamName = teamName;
        Messagas = messagas;
    }

    public int ChatId
    {
        get;
    }

    public string TeamName
    {
        get;
    }

    public List<MessageData> Messagas
    {
        get;
    }
}