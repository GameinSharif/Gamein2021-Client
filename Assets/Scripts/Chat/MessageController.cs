using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public GameObject reportButton;
    public GameObject showReportButtonButton;
    
    public RTLTextMeshPro text;

    private MessageData _messageData;
    private int _chatId;
    private bool _isReported;

    
    private void OnEnable()
    {
        EventManager.Instance.OnReportMessageResponseEvent += OnReportMessageResponseReceived;
    }
 
    private void OnDisable()
    {
        EventManager.Instance.OnReportMessageResponseEvent -= OnReportMessageResponseReceived;
    }
    
    public void SetInfo(string value, MessageData messageData, int chatId)
    {
        text.text = value;
        _messageData = messageData;
        _chatId = chatId;
    }

    public void OnShowReportButtonClicked()
    {
        StartCoroutine(ShowReportButton());
    }
    
    private IEnumerator ShowReportButton()
    {
        reportButton.SetActive(true);
        showReportButtonButton.SetActive(false);
        yield return new WaitForSeconds(5f);
        reportButton.SetActive(false);
        showReportButtonButton.SetActive(true);
    }

    public void OnReportButtonClicked()
    {
        if (_isReported)
        {
            DialogManager.Instance.ShowErrorDialog("already_reported_message_error");
        }
        else
        {
            ReportMessageRequest reportMessageRequest = new ReportMessageRequest(_chatId, text.text, _messageData.senderTeamId, _messageData.insertedAt);
            RequestManager.Instance.SendRequest(reportMessageRequest);
        }
    }

    private bool IsTheSameMessage(MessageData responseMessage)
    {
        bool isTheSame = _messageData.insertedAt == responseMessage.insertedAt
                         && _messageData.text == responseMessage.text
                         && _messageData.senderTeamId == responseMessage.senderTeamId
                         && _messageData.receiverTeamId == responseMessage.receiverTeamId;
        return isTheSame;
    }
    
    private void OnReportMessageResponseReceived(ReportMessageResponse reportMessageResponse)
    {
        if (IsTheSameMessage(reportMessageResponse.message))
        {
            if (reportMessageResponse.result == "Successful")
            {
                showReportButtonButton.SetActive(false);
                _isReported = true;
                string senderTeamName =
                    GameDataManager.Instance.GetTeamName(reportMessageResponse.message.senderTeamId);
                NotificationsController.Instance.AddNewNotification("notification_report_message", senderTeamName);
            }
            else
            {
                DialogManager.Instance.ShowErrorDialog();
            }
        }
    }
    
}
