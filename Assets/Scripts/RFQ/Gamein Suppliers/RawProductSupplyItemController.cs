using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class RawProductSupplyItemController : MonoBehaviour
{
    public RTLTextMeshPro supplierName;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro distance;

    public GameObject showOnMapButton;
    public GameObject makeADealWithSupplierButton;

    private Utils.WeekSupply _weekSupply;

    public void SetInfo(Utils.WeekSupply weekSupply)
    {
        supplierName.text = GameDataManager.Instance.GetSupplierById(weekSupply.supplierId).name;
        costPerUnit.text = weekSupply.price.ToString("0.00");

        Vector2 sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.SUPPLIER, weekSupply.supplierId);
        Vector2 destinationLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.FACTORY, PlayerPrefs.GetInt("FactoryId"));

        distance.text = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation) + "KM";

        _weekSupply = weekSupply;
    }

    public void OnShowOnMapButtonClicked()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        float x = (float)GameDataManager.Instance.GetSupplierById(_weekSupply.supplierId).latitude;
        float y = (float)GameDataManager.Instance.GetSupplierById(_weekSupply.supplierId).longitude;
        MapManager.SnapToLocaltionOnOpenMap = new Vector2(x, y);

        MainMenuManager.Instance.OpenPage(0);
    }

    public void OnMakeADealWithSupplierButtonClicked()
    {
        MakeADealWithSupplierPopupController.Instance.OnOpenMakeADealPopupClick(_weekSupply);
    }

}
