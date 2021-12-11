using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPageController : MonoBehaviour
{

    public static ChatPageController Instance;

    public const int MAX_MESSAGES = 20;
    private EnhancedPoolingSystem<MessageData> _poolingSystem;
    private ChatData _chatData;
    private int _theirTeamId; //only to initialize chat from negotiation

    public int CurrentChatId => _chatData == null ? _theirTeamId : _chatData.TheirTeamId;

    public GameObject chatPage;
    public RectTransform chatScrollPanel;
    public GameObject myMessagePrefab;
    public GameObject theirMessagePrefab;
    public TMP_InputField inputField;
    public RTLTextMeshPro teamName;
    public Image teamAvatar;
    public ScrollRect chatScrollViewScrollRect;

    private void Awake()
    {
        Instance = this;
        _poolingSystem = new EnhancedPoolingSystem<MessageData>(chatScrollPanel, new[]{myMessagePrefab, theirMessagePrefab}, MessageInitializer, MAX_MESSAGES);
    }

    private void MessageInitializer(GameObject theGameObject, int indexOfPrefab, int indexInParent, MessageData messageData)
    {
        var controller = theGameObject.GetComponent<MessageController>();
        controller.SetInfo(messageData.text, messageData, CurrentChatId);
    }

    public void AddMessageToChat(MessageData messageData)
    {
        if (_poolingSystem.ActiveCount >= MAX_MESSAGES)
        {
            _poolingSystem.Remove(0);
        }
        _poolingSystem.Add(messageData.IsFromMe ? 0 : 1, messageData);

        RebuildLayout();
        StartCoroutine(RebuildCoroutine());
    }

    private void RebuildLayout()
    {
        Canvas.ForceUpdateCanvases();
        chatScrollViewScrollRect.verticalNormalizedPosition = 0.0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatScrollPanel);
    }

    private IEnumerator RebuildCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        RebuildLayout();
    }

    public void OnSendMessageClicked()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }

        SendMessageToServer(inputField.text);
        
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void SendMessageToServer(string text)
    {
        if (_chatData == null) 
        {
            RequestManager.Instance.SendRequest(new NewMessageRequest(_theirTeamId, text));

        }
        else
        {
            RequestManager.Instance.SendRequest(new NewMessageRequest(_chatData.TheirTeamId, text));
        }
    }

    public void LoadChat(ChatData chatData)
    {
        chatPage.SetActive(true); 
        
        _chatData = chatData;
        teamName.text = chatData.TeamName;
        
        _poolingSystem.RemoveAll();
        foreach (var messageData in chatData.messages)
        {
            _poolingSystem.Add(messageData.IsFromMe ? 0 : 1, messageData);
        }

        SetOpen(true);
        RebuildLayout();
        StartCoroutine(RebuildCoroutine());
    }

    public void LoadChat(int otherTeamId)
    {
        chatPage.SetActive(true);

        _chatData = null;
        _theirTeamId = otherTeamId;
        teamName.text = GameDataManager.Instance.GetTeamName(otherTeamId);

        _poolingSystem.RemoveAll();

        RebuildLayout();
        SetOpen(true);
    }

    public void OnBackButtonPressed()
    {
        SetOpen(false);
    }

    private void SetOpen(bool value)
    {
        chatPage.SetActive(value);
    }

    public void OnCloseButtonClicked()
    {
        SetOpen(false);
        ChatsListController.Instance.OnCloseButtonCLicked();
    }
}
