using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class RawProductSupplyItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    public RTLTextMeshPro costPerUnit;

    public GameObject showOnMapButton;
    public GameObject makeADealWithSupplierButton;

    private Utils.WeekSupply _weekSupply;

    public void SetInfo(int no, string supplierName, string costPerUnit)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        this.costPerUnit.text = costPerUnit;
    }

    public void SetInfo(int no, Utils.WeekSupply weekSupply)
    {
        SetInfo(
            no: no,
            supplierName: GameDataManager.Instance.GetSupplierById(weekSupply.supplierId).name,
            costPerUnit: weekSupply.price.ToString("0.00")
        );
        
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

        MainMenuManager.Instance.IsInTradePage = false;
        MainMenuManager.Instance.OnLoadMapSceneButtonClick();
    }

    public void OnMakeADealWithSupplierButtonClicked()
    {
        MakeADealWithSupplierPopupController.Instance.OnOpenMakeADealPopupClick(_weekSupply);
    }

}
