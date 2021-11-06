using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class RawProductSupplyItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    //public Localize productNameLocalize;
    public RTLTextMeshPro costPerUnit;

    public GameObject showOnMapButton;
    public GameObject makeADealWithSupplierButton;

    private Utils.WeekSupply _weekSupply;

    public void SetInfo(int no, string supplierName, string costPerUnit)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        //productNameLocalize.SetKey("product_" + productNameKey);
        this.costPerUnit.text = costPerUnit;
    }

    public void SetInfo(int no, Utils.WeekSupply weekSupply)
    {
        SetInfo(
            no: no,
            supplierName: weekSupply.supplier.name,
            //productNameKey: GameDataManager.Instance.GetProductById(offer.productId).name,
            costPerUnit: weekSupply.price + "$"
        );
        
        _weekSupply = weekSupply;
    }

    public void OnShowOnMapButtonClicked()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        float x = (float)_weekSupply.supplier.latitude;
        float y = (float)_weekSupply.supplier.longitude;
        MapManager.SnapToLocaltionOnOpenMap = new Vector2(x, y);

        MainMenuManager.Instance.OnLoadMapSceneButtonClick();

        GameinSuppliersController.Instance.SetGameinSuppliersCanvasActive(false);
    }

    public void OnMakeADealWithSupplierButtonClicked()
    {
        MakeADealWithSupplierPopupController.Instance.OnOpenMakeADealPopupClick(_weekSupply);
    }

}
