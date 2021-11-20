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
    public Transform chatScrollPanel;
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
        controller.SetText(messageData.text);
    }

    public void AddMessageToChat(MessageData messageData)
    {
        if (_poolingSystem.ActiveCount >= MAX_MESSAGES)
        {
            _poolingSystem.Remove(0);
        }
        _poolingSystem.Add(messageData.IsFromMe ? 0 : 1, messageData);

        RebuildLayout();
    }

    private void RebuildLayout()
    {
        Canvas.ForceUpdateCanvases();
        chatScrollViewScrollRect.verticalNormalizedPosition = 0.0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatScrollPanel as RectTransform);
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
        chatPage.SetActive(true); // do not delete this; if scroll panel is not active, all children are not active as well and pooling system won't work 
        
        _chatData = chatData;
        teamName.text = chatData.TeamName;
        //TODO set avatar
        
        _poolingSystem.RemoveAll();
        foreach (var messageData in chatData.messages)
        {
            _poolingSystem.Add(messageData.IsFromMe ? 0 : 1, messageData);
        }
        
        RebuildLayout();
        SetOpen(true);
    }

    public void LoadChat(int otherTeamId)
    {
        chatPage.SetActive(true); // do not delete this; if scroll panel is not active, all children are not active as well and pooling system won't work 

        _chatData = null;
        _theirTeamId = otherTeamId;
        teamName.text = GameDataManager.Instance.GetTeamName(otherTeamId);
        //TODO set avatar

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
        //TODO show animation
        chatPage.SetActive(value);
    }
}
