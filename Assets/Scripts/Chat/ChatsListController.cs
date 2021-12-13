using System;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatsListController : MonoBehaviour
{
    public static ChatsListController Instance;
    
    private Dictionary<int, ChatData> _chatDataOfChatId;
    private Dictionary<int, ChatItemController> _controllerOfChatId;

    public GameObject chatParentGameObject;

    public Transform chatsListScrollPanel;
    public GameObject chatItemPrefab;

    public GameObject hint;
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

        if (response.chats.Count == 0)
        {
            Debug.Log("No Chat");
            hint.SetActive(true);
        }
        else
        {
            hint.SetActive(false);
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

        Debug.Log(newMessageResponse.chat.TheirTeamId);
        Debug.Log(newMessageResponse.message.TheirTeamId);

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
            AddAndInitializeChatItem(chat.TheirTeamId, chat.TeamName, chat.messages[chat.messages.Count - 1].text);
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

    public void ToggleChatParent()
    {
        chatParentGameObject.SetActive(!chatParentGameObject.activeSelf);
    }

    public void OpenChatFromNegotiation(int otherTeamId)
    {
        chatParentGameObject.SetActive(true);
        if (_chatDataOfChatId.ContainsKey(otherTeamId))
        {
            ShowChatPageWithId(_chatDataOfChatId[otherTeamId].TheirTeamId);
        }
        else
        {
            ChatPageController.Instance.LoadChat(otherTeamId);
        }
    }

    public void OnCloseButtonCLicked()
    {
        chatParentGameObject.SetActive(false);
    }

    public void OnSearchBarValueChanged(string query)
    {
        query ??= "";

        foreach (var controller in _controllerOfChatId.Values)
        {
            controller.gameObject.SetActive(controller.teamName.text.Contains(query));
        }

        RebuildListLayout();
    }

    private void RebuildListLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatsListScrollPanel as RectTransform);
    }
}