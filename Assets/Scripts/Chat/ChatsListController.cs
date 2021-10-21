using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatsListController : MonoBehaviour
{
    public static ChatsListController Instance;
    
    private Dictionary<int, ChatData> _chatDataOfChatId;
    private Dictionary<int, ChatItemController> _controllerOfChatId;

    public Transform chatsListScrollPanel;
    public GameObject chatItemPrefab;
    private void Awake()
    {
        Instance = this;
        _chatDataOfChatId = new Dictionary<int, ChatData>();
        _controllerOfChatId = new Dictionary<int, ChatItemController>();

        foreach (Transform child in chatsListScrollPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetAllChatsResponseEvent += OnGetAllChatsResponse;
        EventManager.Instance.OnNewMessageResponseEvent += OnNewMessageResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetAllChatsResponseEvent -= OnGetAllChatsResponse;
        EventManager.Instance.OnNewMessageResponseEvent -= OnNewMessageResponse;
    }

    private void Start()
    {
        //test
        for (int i = 1; i <= 20; i++)
        {
            TestAddChatItem(i, "Team #" + i);
        }
    }

    //test
    private void TestAddChatItem(int chatId, string teamName)
    {
        var list = new List<MessageData>
        {
            new MessageData{text = "hello\nwe are team " + teamName, IsFromMe = false},
            new MessageData{text = "hey", IsFromMe = true},
            new MessageData{text = "good for you\nnow get out", IsFromMe = true}
        };

        var chatData = new ChatData {id = chatId, TeamName = teamName, messages = list};
        _chatDataOfChatId.Add(chatId, chatData);
        
        var itemController = UnityEngine.Object.Instantiate(chatItemPrefab, chatsListScrollPanel).GetComponent<ChatItemController>();
        _controllerOfChatId.Add(chatId, itemController);
        
        itemController.SetData(chatId, teamName);
        itemController.SetLastMessagePreview(list[list.Count - 1].text);
        itemController.UnreadCount = 3;
    }

    public void ShowChatPageWithId(int id)
    {
        _controllerOfChatId[id].UnreadCount = 0;
        ChatPageController.Instance.LoadChat(_chatDataOfChatId[id]);
    }

    private void OnGetAllChatsResponse(GetAllChatsResponse response)
    {
        _chatDataOfChatId.Clear();
        foreach (ChatData chat in response.chats)
        {
            _chatDataOfChatId.Add(chat.TheirTeamId, chat);
        }
    }

    public void OnOpenChatsList()
    {
        foreach (ChatData chat in _chatDataOfChatId.Values)
        {
            if (!_controllerOfChatId.ContainsKey(chat.TheirTeamId))
            {
                AddAndInitializeChatItem(chat.TheirTeamId, chat.TeamName);
            }
        }
    }

    private void AddAndInitializeChatItem(int chatId, string teamName)
    {
        var itemController = Instantiate(chatItemPrefab, chatsListScrollPanel).GetComponent<ChatItemController>();
        itemController.SetData(chatId, teamName);
        _controllerOfChatId.Add(chatId, itemController);
    }

    private void OnNewMessageResponse(NewMessageResponse newMessageResponse)
    {
        if (_chatDataOfChatId.ContainsKey(newMessageResponse.messageDto.TheirTeamId))
        {
            var messages = _chatDataOfChatId[newMessageResponse.messageDto.TheirTeamId].messages;
            if (messages.Count >= ChatPageController.MAX_MESSAGES)
            {
                messages.RemoveAt(0);
            }
            messages.Add(newMessageResponse.messageDto);
        }
        else
        {
            //TODO add chat data
        }

        if (ChatPageController.Instance.CurrentChatId != newMessageResponse.messageDto.TheirTeamId &&
            _controllerOfChatId.ContainsKey(newMessageResponse.messageDto.TheirTeamId))
        {
            var controller = _controllerOfChatId[newMessageResponse.messageDto.TheirTeamId];
            controller.UnreadCount++;
            controller.SetLastMessagePreview(newMessageResponse.messageDto.text);
        }

        if (ChatPageController.Instance.CurrentChatId == newMessageResponse.messageDto.TheirTeamId) // is in chat page
        {
            ChatPageController.Instance.AddMessageToChat(newMessageResponse.messageDto);
        }
    }
}