using System;
using UnityEngine;

public class DCManager : MonoBehaviour
{
    public static DCManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnBuyDCResponseEvent += OnBuyDCResponse;
        EventManager.Instance.OnSellDCResponseEvent += OnSellDCResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBuyDCResponseEvent -= OnBuyDCResponse;
        EventManager.Instance.OnSellDCResponseEvent -= OnSellDCResponse;
    }

    private void OnBuyDCResponse(BuyDCResponse response)
    {
        if (response.dc == null)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        UpdateGameData(response.dc);
        MainHeaderManager.Instance.Money -= response.dc.buyingPrice;
        
        if (IsInMapScene())
        {
            MapManager.Instance.UpdateDcMarker(response.dc, false);
        }

        NotificationsController.Instance.AddNewNotification("notification_DC_bought", response.dc.name);
    }

    private void OnSellDCResponse(SellDCResponse response)
    {
        if (response.dc == null)
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        UpdateGameData(response.dc);
        MainHeaderManager.Instance.Money += response.dc.sellingPrice;

        if (IsInMapScene())
        {
            MapManager.Instance.UpdateDcMarker(response.dc, true);
        }

        NotificationsController.Instance.AddNewNotification("notification_DC_sold", response.dc.name);
    }

    private void UpdateGameData(Utils.DC dc)
    {
        Utils.DC oldDto = GameDataManager.Instance.DCs.Find(dto => dto.id == dc.id);
        GameDataManager.Instance.DCs.Remove(oldDto);
        GameDataManager.Instance.DCs.Add(dc);
    }

    private bool IsInMapScene()
    {
        return MapManager.IsInMap;
    }
}