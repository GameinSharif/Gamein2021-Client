using System;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class ChatItemController : MonoBehaviour
{
    public RTLTextMeshPro teamName;
    public GameObject unreadCountObject;
    public RTLTextMeshPro lastMessagePreview;
    
    private int _chatId;
    private string _teamNameString;

    public string TeamNameString => _teamNameString;

    private int _unreadCount = 0;
    public int UnreadCount
    {
        get => _unreadCount;
        set
        {
            _unreadCount = value;
            if (_unreadCount == 0)
            {
                unreadCountObject.SetActive(false);
                return;
            }

            unreadCountObject.SetActive(true);
        }
    }

    public void SetData(int chatId, string theTeamName)
    {
        _chatId = chatId;
        _teamNameString = theTeamName;
        teamName.text = theTeamName;
        lastMessagePreview.text = "";
        //TODO set avatar
    }

    public void OnChatItemClicked()
    {
        ChatsListController.Instance.ShowChatPageWithId(_chatId);
    }

    public void SetLastMessagePreview(string text)
    {
        lastMessagePreview.text = text.Replace('\n', ' ');
    }
}