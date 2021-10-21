using System;

[Serializable]
public class GetAllChatsRequest : RequestObject
{
    public GetAllChatsRequest() : base(RequestTypeConstant.GET_All_CHATS)
    {
        
    }
}