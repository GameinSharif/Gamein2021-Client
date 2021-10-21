using System;

[Serializable]
public class NewMessageResponse : ResponseObject
{
    public MessageData messageDto;
    public string message;
}