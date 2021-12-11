using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationItemController : MonoBehaviour
{
    public Localize notificationTextLocalize;
    private int _itemsGameObjectIndex;

    public void SetInfo(string textLocalize, int index, params string[] replaceStrings)
    {
        notificationTextLocalize.SetKey(textLocalize, replaceStrings);
        _itemsGameObjectIndex = index;
    }

    public void ChangeIndex(int index)
    {
        _itemsGameObjectIndex = index;
    }
}
