using System.Dynamic;
using RTLTMPro;
using UnityEngine;

public class StorageProductController : MonoBehaviour
{
    public RTLTextMeshPro name;
    public RTLTextMeshPro available;
    public RTLTextMeshPro coming;
    public RTLTextMeshPro total;

    private int _index;
    private Utils.StorageType _storageType;

    public void SetInfo(int index, Utils.StorageType storageType)
    {
        _index = index;
        _storageType = storageType;
    }

    public void OnClicked()
    {
        switch (_storageType)
        {
            case Utils.StorageType.DC:
                DcTabController.Instance.OnDcProductClicked(_index);
                break;
            case Utils.StorageType.WAREHOUSE:
                WarehouseTabController.Instance.OnWarehouseProductClicked(_index);
                break;
        }
    }
}