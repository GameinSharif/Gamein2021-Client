using System;

public class MessageData
{
    public string Text
    {
        get;
    }

    public bool IsFromMe
    {
        get;
    }

    public CustomDateTime DateTime
    {
        get;
    }

    public MessageData(string text, bool isFromMe, CustomDateTime dateTime)
    {
        Text = text;
        IsFromMe = isFromMe;
        DateTime = dateTime;
    }
}