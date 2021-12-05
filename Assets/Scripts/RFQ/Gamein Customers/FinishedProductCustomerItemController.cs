using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class FinishedProductCustomerItemController : MonoBehaviour
{
    public RTLTextMeshPro customerName;
    public RTLTextMeshPro amount;
    public RTLTextMeshPro distance;

    public GameObject showOnMapButton;
    public GameObject makeADealWithCustomerButton;

    private Utils.WeekDemand _weekDemand;
    private Utils.Storage _storage;

    public void SetInfo(Utils.WeekDemand weekDemand, Utils.Storage storage)
    {
        _weekDemand = weekDemand;
        _storage = storage;

        customerName.text = GameDataManager.Instance.GetCustomerById(weekDemand.gameinCustomerId).name;
        amount.text = weekDemand.amount.ToString();

        CalculateDistance();
    }

    private void CalculateDistance()
    {
        Vector2 sourceLocation = GameDataManager.Instance.GetLocationByTypeAndId(_storage.dc ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY, _storage.buildingId);
        Vector2 destinationLocation = GameDataManager.Instance.GetLocationByTypeAndId(Utils.TransportNodeType.GAMEIN_CUSTOMER, _weekDemand.gameinCustomerId);

        distance.text = TransportManager.Instance.GetTransportDistance(sourceLocation, destinationLocation) + "KM";
    }

    public void OnShowOnMapButtonClicked()
    {
        float x = (float)GameDataManager.Instance.GetCustomerById(_weekDemand.gameinCustomerId).latitude;
        float y = (float)GameDataManager.Instance.GetCustomerById(_weekDemand.gameinCustomerId).longitude;
        MapManager.SnapToLocaltionOnOpenMap = new Vector2(x, y);

        MainMenuManager.Instance.OpenPage(0);
    }

    public void OnMakeADealWithDemanderButtonClicked()
    {
        NewContractController.Instance.OnOpenMakeADealPopupClick(_weekDemand);
    }

    public void OnChangeSourceStorage(Utils.Storage storage)
    {
        _storage = storage;

        CalculateDistance();
    }

}
