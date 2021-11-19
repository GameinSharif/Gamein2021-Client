using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatsListController : MonoBehaviour
{
    public static ChatsListController Instance;
    
    private Dictionary<int, ChatData> _chatDataOfChatId;
    private Dictionary<int, ChatItemController> _controllerOfChatId;

    public GameObject chatsListGameObject;

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
                AddAndInitializeChatItem(chat.TheirTeamId, chat.TeamName, chat.messages[chat.messages.Count - 1].text);
            }
        }
    }

    private void AddAndInitializeChatItem(int chatId, string teamName, string lastMessage)
    {
        var itemController = Instantiate(chatItemPrefab, chatsListScrollPanel).GetComponent<ChatItemController>();
        itemController.SetData(chatId, teamName);
        itemController.SetLastMessagePreview(lastMessage);
        _controllerOfChatId.Add(chatId, itemController);
    }

    private void OnNewMessageResponse(NewMessageResponse newMessageResponse)
    {
        if (newMessageResponse.chat == null)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        if (_chatDataOfChatId.ContainsKey(newMessageResponse.message.TheirTeamId))
        {
            var messages = _chatDataOfChatId[newMessageResponse.message.TheirTeamId].messages;
            if (messages.Count >= ChatPageController.MAX_MESSAGES)
            {
                messages.RemoveAt(0);
            }
            messages.Add(newMessageResponse.message);
        }
        else
        {
            ChatData chat = newMessageResponse.chat;
            _chatDataOfChatId.Add(chat.TheirTeamId, chat);
        }

        if (ChatPageController.Instance.CurrentChatId != newMessageResponse.message.TheirTeamId &&
            _controllerOfChatId.ContainsKey(newMessageResponse.message.TheirTeamId))
        {
            var controller = _controllerOfChatId[newMessageResponse.message.TheirTeamId];
            controller.UnreadCount++;
            controller.SetLastMessagePreview(newMessageResponse.message.text);
        }

        if (ChatPageController.Instance.CurrentChatId == newMessageResponse.message.TheirTeamId) // is in chat page
        {
            ChatPageController.Instance.AddMessageToChat(newMessageResponse.message);
        }
    }

    public void ToggleChatsList()
    {
        chatsListGameObject.SetActive(!chatsListGameObject.activeSelf);
    }
}