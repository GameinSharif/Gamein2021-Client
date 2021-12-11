using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationsController : MonoBehaviour
{
    public static NotificationsController Instance;
    public const int MAX_NOTIFICATIONS = 20;

    public GameObject newMessageSign;
    public GameObject notificationsParentGameObject;

    public GameObject notificationsScrollPanel;
    
    public GameObject notificationItemPrefab;
    private List<GameObject> _spawnedNotificationGameObjects = new List<GameObject>();
    private List<string> _activeNotificationsTextLocalize = new List<string>();
    private List<string> _activeNotificationsTextLocalizeParams = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public void AddNewNotification(string textLocalize, string param)
    {
        if (_activeNotificationsTextLocalize.Count == MAX_NOTIFICATIONS)
        {
            CloseNotificationItem(0);
        }
        _activeNotificationsTextLocalize.Add(textLocalize);
        _activeNotificationsTextLocalizeParams.Add(param);
        if (notificationsParentGameObject.activeSelf)
        {
            int index = _activeNotificationsTextLocalize.Count - 1;
            AddNotificationItemToList(index, _activeNotificationsTextLocalize[index], _activeNotificationsTextLocalizeParams[index]);
            Canvas.ForceUpdateCanvases();
        }
        else
        {
            newMessageSign.SetActive(true);
        }
    }

    private void ShowNotifications()
    {
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _activeNotificationsTextLocalize.Count; i++)
        {
            AddNotificationItemToList(i, _activeNotificationsTextLocalize[i], _activeNotificationsTextLocalizeParams[i]);
        }
    }

    private void AddNotificationItemToList(int index, string textLocalize, string param)
    {
        GameObject createdItem = GetItem(notificationsScrollPanel);
        createdItem.transform.SetSiblingIndex(index + 1);

        NotificationItemController controller = createdItem.GetComponent<NotificationItemController>();
        controller.SetInfo(textLocalize, index, param);

        createdItem.SetActive(true);
    }
    
    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedNotificationGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(notificationItemPrefab, parent.transform);
        _spawnedNotificationGameObjects.Add(newItem);

        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedNotificationGameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void CloseNotificationItem(int index)
    {
        for (int i = 0; i < _activeNotificationsTextLocalize.Count; i++)
        {
            if (index == i)
            {
                _activeNotificationsTextLocalize.RemoveAt(i);
                _activeNotificationsTextLocalizeParams.RemoveAt(i);
                _spawnedNotificationGameObjects[i].SetActive(false);
            }
            else if (index < i)
            {
                _spawnedNotificationGameObjects[i].GetComponent<NotificationItemController>().ChangeIndex(i - 1);
            }
        }
    }
    
    public void OnNotificationsButtonClick()
    {
        Debug.Log(notificationsParentGameObject.activeSelf);
        newMessageSign.SetActive(false);
        if (!notificationsParentGameObject.activeSelf)
        {
            ShowNotifications();
        }
        notificationsParentGameObject.SetActive(!notificationsParentGameObject.activeSelf);
    }

    public void OnCloseNotificationsButtonClick()
    {
        notificationsParentGameObject.SetActive(false);
    }
}
