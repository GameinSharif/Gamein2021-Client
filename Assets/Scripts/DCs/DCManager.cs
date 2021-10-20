using System;
using System.ComponentModel.Design.Serialization;
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
        UpdateGameData(response.dcDto);
        
        //TODO
        // if (isInMap)
        // {
        //     MapManager.Instance.UpdateDto();
        // }
    }

    private void OnSellDCResponse(SellDCResponse response)
    {
        UpdateGameData(response.dcDto);
        
        //TODO
        // if (isInMap)
        // {
        //     MapManager.Instance.UpdateDto();
        // }
    }

    private void UpdateGameData(Utils.DCDto dcDto)
    {
        Utils.DCDto oldDto = GameDataManager.Instance.DCDtos.Find(dto => dto.DCId == dcDto.DCId);
        GameDataManager.Instance.DCDtos.Remove(oldDto);
        GameDataManager.Instance.DCDtos.Add(dcDto);
    }

    public void SendBuyRequest(Utils.DCDto dcDto)
    {
        RequestManager.Instance.SendRequest(new BuyDCRequest(RequestTypeConstant.BUY_DC, dcDto.DCId));
    }

    public void SendSellRequest(Utils.DCDto dcDto)
    {
        RequestManager.Instance.SendRequest(new SellDCRequest(RequestTypeConstant.BUY_DC, dcDto.DCId));
    }
}