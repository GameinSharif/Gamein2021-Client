using RTLTMPro;
using UnityEngine;
public class StorageTabItemController : MonoBehaviour
{
    public RTLTextMeshPro name;

    private int _index;

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void OnValueChanged(bool value)
    {
        if (value)
        {
            StorageTabSelector.Instance.OnTabClicked(_index);
        }
    }
}