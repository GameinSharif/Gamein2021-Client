using System;

[Serializable]
public class NewMessageResponse : ResponseObject
{
    public ChatData chat;
    public MessageData message;
    public string result;
}