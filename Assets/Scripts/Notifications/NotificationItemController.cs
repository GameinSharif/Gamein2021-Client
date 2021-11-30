using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationItemController : MonoBehaviour
{
    public Localize notificationTextLocalize;
    private int _itemsGameObjectIndex;

    public void SetInfo(string textLocalize, int index)
    {
        notificationTextLocalize.SetKey(textLocalize, index.ToString());
        _itemsGameObjectIndex = index;
    }

    public void ChangeIndex(int index)
    {
        _itemsGameObjectIndex = index;
    }

    public void OnCloseNotificationItemButtonClick()
    {
        Debug.Log("closing");
        NotificationsController.Instance.CloseNotificationItem(_itemsGameObjectIndex);
    }
}
