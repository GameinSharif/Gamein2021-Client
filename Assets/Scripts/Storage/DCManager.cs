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

    private void UpdateGameData(Utils.DC dcDto)
    {
        Utils.DC oldDto = GameDataManager.Instance.DCs.Find(dto => dto.id == dcDto.id);
        GameDataManager.Instance.DCs.Remove(oldDto);
        GameDataManager.Instance.DCs.Add(dcDto);
    }

    private bool IsInMapScene()
    {
        return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name.Contains("Map");
    }
}