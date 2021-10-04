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

    private const int MAX_MESSAGES = 20;
    private EnhancedPoolingSystem<MessageData> _poolingSystem;
    private ChatData _chatData;

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

    private void OnEnable()
    {
        //TODO add event handling action
    }
    
    private void OnDisable()
    {
        //TODO add event handling action
    }

    private void MessageInitializer(GameObject theGameObject, int indexOfPrefab, int indexInParent, MessageData messageData)
    {
        var controller = theGameObject.GetComponent<MessageController>();
        controller.SetText(messageData.Text);
    }

    private void AddMessageToChat(MessageData messageData)
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
        
        //TODO send message to server
        
        AddMessageToChat(new MessageData(inputField.text, true, null));
        AddMessageToChat(new MessageData("wow! that is amazing :) ", false, null));
        
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void OnMessageReceived(MessageData messageData)
    {
        //TODO receive message from server and show to user
        //if the new message is from this chat:
        // AddMessageToChat(messageData);
    }

    public void LoadChat(ChatData chatData)
    {
        chatPage.SetActive(true); // do not delete this; if scroll panel is not active, all children are not active as well and pooling system won't work 
        
        _chatData = chatData;
        teamName.text = chatData.TeamName;
        //TODO set avatar
        
        _poolingSystem.RemoveAll();
        foreach (var messageData in chatData.Messagas)
        {
            _poolingSystem.Add(messageData.IsFromMe ? 0 : 1, messageData);
        }
        
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
