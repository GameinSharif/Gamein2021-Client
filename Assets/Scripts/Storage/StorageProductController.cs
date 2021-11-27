using System.Dynamic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageProductController : MonoBehaviour
{
    public Localize nameLocalize;
    public RTLTextMeshPro available;
    public RTLTextMeshPro coming;
    public RTLTextMeshPro total;
    public Image productImage;

    private Utils.Product _product;
    private Utils.StorageType _storageType;

    public Utils.Product Product => _product;

    public void SetData(Utils.Product product, Utils.StorageType storageType, int availableAmount, int comingAmount)
    {
        _product = product;
        _storageType = storageType;
        
        nameLocalize.SetKey("product_" + _product.name);
        available.text = availableAmount.ToString();
        coming.text = comingAmount.ToString();
        total.text = (availableAmount + comingAmount).ToString();
        
        //TODO set product image
    }

    public void OnRemoveButtonClicked()
    {
        switch (_storageType)
        {
            case Utils.StorageType.DC:
                DcTabController.Instance.OnProductRemoveClicked(_product);
                break;
            case Utils.StorageType.WAREHOUSE:
                WarehouseTabController.Instance.OnProductRemoveClicked(_product);
                break;
        }
    }

    public void OnTransportButtonClicked()
    {
        switch (_storageType)
        {
            case Utils.StorageType.DC:
                DcTabController.Instance.OnProductTransportClicked(_product);
                break;
            case Utils.StorageType.WAREHOUSE:
                WarehouseTabController.Instance.OnProductTransportClicked(_product);
                break;
        }
    }
}