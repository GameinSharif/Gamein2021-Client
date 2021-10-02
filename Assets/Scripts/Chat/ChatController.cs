using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChatController : MonoBehaviour
{

    public static readonly int MAX_MESSAGES = 50; 
    
    private PoolingSystem<MessageData> poolingSystem;
    public Transform chatScrollPanel;
    public GameObject messagePrefab;
    public TMP_InputField inputField;
    public RTLTextMeshPro teamName;
    public Image teamAvatar;
    public ScrollRect chatScrollViewScrollRect;

    private void Awake()
    {
        poolingSystem = new PoolingSystem<MessageData>(chatScrollPanel, messagePrefab, MessageInitializer, MAX_MESSAGES);
    }

    private void OnEnable()
    {
        //TODO add event handling action
    }
    
    private void OnDisable()
    {
        //TODO add event handling action
    }

    private void MessageInitializer(GameObject theGameObject, int index, MessageData messageData)
    {
        var controller = theGameObject.GetComponent<MessageController>();
        controller.SetText(messageData.Text);
        controller.IsFromMe = messageData.IsFromMe;
    }

    private void AddMessageToChat(MessageData messageData)
    {
        if (poolingSystem.ActiveCount >= MAX_MESSAGES)
        {
            poolingSystem.Remove(0);
        }
        poolingSystem.Add(messageData);
        
        Canvas.ForceUpdateCanvases();
        chatScrollViewScrollRect.verticalNormalizedPosition = 0f;
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
    }

    public void OnMessageReceived(MessageData messageData)
    {
        //TODO receive message from server and show to user
        // AddMessageToChat(messageData);
    }
}
