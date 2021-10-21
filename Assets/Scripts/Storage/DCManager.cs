using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        UpdateGameData(response.dcDto);
        
        if (IsInMapScene())
        {
            MapManager.Instance.UpdateDtoMarker(response.dcDto, false);
        }
    }

    private void OnSellDCResponse(SellDCResponse response)
    {
        UpdateGameData(response.dcDto);
        
        if (IsInMapScene())
        {
            MapManager.Instance.UpdateDtoMarker(response.dcDto, true);
        }
    }

    private void UpdateGameData(Utils.DCDto dcDto)
    {
        Utils.DCDto oldDto = GameDataManager.Instance.DCDtos.Find(dto => dto.DCId == dcDto.DCId);
        GameDataManager.Instance.DCDtos.Remove(oldDto);
        GameDataManager.Instance.DCDtos.Add(dcDto);
    }

    private bool IsInMapScene()
    {
        return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name.Contains("Map");
    }

    public void SendBuyRequest(Utils.DCDto dcDto)
    {
        RequestManager.Instance.SendRequest(new BuyDCRequest(RequestTypeConstant.BUY_DC, dcDto.DCId));
    }

    public void SendSellRequest(Utils.DCDto dcDto)
    {
        RequestManager.Instance.SendRequest(new SellDCRequest(RequestTypeConstant.SELL_DC, dcDto.DCId));
    }
}