using System;
using System.Collections.Generic;

[Serializable]
public class GetAllChatsResponse : ResponseObject
{
    public List<ChatData> chats;
}