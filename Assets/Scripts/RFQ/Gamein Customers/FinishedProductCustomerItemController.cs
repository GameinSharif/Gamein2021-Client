using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class FinishedProductCustomerItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro customerName;
    public RTLTextMeshPro amount;

    public GameObject showOnMapButton;
    public GameObject makeADealWithCustomerButton;

    private Utils.WeekDemand _weekDemand;

    public void SetInfo(int no, string DemanderName, string amount)
    {
        this.no.text = no.ToString();
        this.customerName.text = DemanderName;
        this.amount.text = amount;
    }

    public void SetInfo(int no, Utils.WeekDemand weekDemand)
    {
        SetInfo(
            no: no,
            DemanderName: GameDataManager.Instance.GetCustomerById(weekDemand.gameinCustomerId).name,
            amount: weekDemand.amount.ToString()
        );
        
        _weekDemand = weekDemand;
    }

    public void OnShowOnMapButtonClicked()
    {
        float x = (float)GameDataManager.Instance.GetCustomerById(_weekDemand.gameinCustomerId).latitude;
        float y = (float)GameDataManager.Instance.GetCustomerById(_weekDemand.gameinCustomerId).longitude;
        MapManager.SnapToLocaltionOnOpenMap = new Vector2(x, y);

        MainMenuManager.Instance.IsInTradePage = false;
        MainMenuManager.Instance.OnLoadMapSceneButtonClick();
    }

    public void OnMakeADealWithDemanderButtonClicked()
    {
        MakeADealWithDemanderPopupController.Instance.OnOpenMakeADealPopupClick(_weekDemand);
    }

}
