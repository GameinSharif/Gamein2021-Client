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
            new MessageData("hello\nwe are team " + teamName, false, null),
            new MessageData("hey", true, null),
            new MessageData("good for you", true, null),
        };

        var chatData = new ChatData(chatId, teamName, list);
        _chatDataOfChatId.Add(chatId, chatData);
        
        var itemController = UnityEngine.Object.Instantiate(chatItemPrefab, chatsListScrollPanel).GetComponent<ChatItemController>();
        _controllerOfChatId.Add(chatId, itemController);
        
        itemController.SetData(chatId, teamName);
        itemController.UnreadCount = 3;
    }

    public void ShowChatPageWithId(int id)
    {
        _controllerOfChatId[id].UnreadCount = 0;
        ChatPageController.Instance.LoadChat(_chatDataOfChatId[id]);
    }
}