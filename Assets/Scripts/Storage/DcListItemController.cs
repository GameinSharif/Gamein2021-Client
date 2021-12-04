using RTLTMPro;
using UnityEngine;

public class DcListItemController : MonoBehaviour
{
    public RTLTextMeshPro number;

    private Utils.Storage _storage;

    public void SetInfo(Utils.Storage storage)
    {
        _storage = storage;
        number.text = storage.buildingId.ToString();
    }

    public void OnValueChanged(bool value)
    {
        if (value)
        {
            StorageDashboardController.Instance.OnDcItemClicked(_storage);
        }
    }
}