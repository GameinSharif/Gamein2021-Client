using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationItemController : MonoBehaviour
{
    public Localize notificationTextLocalize;
    private int _itemsGameObjectIndex;

    public void SetInfo(string textLocalize, int index, string param)
    {
        if (param != "")
        {
            notificationTextLocalize.SetKey(textLocalize, param);
        }
        else
        {
            notificationTextLocalize.SetKey(textLocalize);
        }
        _itemsGameObjectIndex = index;
    }

    public void ChangeIndex(int index)
    {
        _itemsGameObjectIndex = index;
    }
}
