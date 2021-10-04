using System;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class ChatItemController : MonoBehaviour
{
    public RTLTextMeshPro teamName;
    public Image teamAvatar;
    public RTLTextMeshPro unreadCountText;
    public GameObject unreadCountObject;
    
    private int _chatId;

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

            unreadCountText.text = _unreadCount.ToString();
            unreadCountObject.SetActive(true);
        }
    }

    public void SetData(int chatId, string theTeamName)
    {
        _chatId = chatId;
        teamName.text = theTeamName;
        //TODO set avatar
    }

    private void SetUnreadCount(int value)
    {
        if (value == 0)
        {
            unreadCountObject.SetActive(false);
            return;
        }

        unreadCountText.text = value.ToString();
        unreadCountObject.SetActive(true);
    }

    public void OnChatItemClicked()
    {
        ChatsListController.Instance.ShowChatPageWithId(_chatId);
    }
}