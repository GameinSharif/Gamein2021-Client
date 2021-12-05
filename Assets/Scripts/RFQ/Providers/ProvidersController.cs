using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;
using UnityEngine.UI;

public class ProvidersController : MonoBehaviour
{
    public static ProvidersController Instance;

    public GameObject myProviderItemPrefab;
    public GameObject otherProviderItemPrefab;

    public RectTransform myProvidersScrollPanel;
    public RectTransform otherProvidersScrollPanel;

    private List<MyProviderItemController> _myTeamProviderItemControllers = new List<MyProviderItemController>();
    private List<OtherProviderItemController> _otherTeamsProviderItemControllers = new List<OtherProviderItemController>();

    private PoolingSystem<Utils.Provider> _myProvidersPool;
    private PoolingSystem<Utils.Provider> _otherProvidersPool;

    private void Awake()
    {
        Instance = this;

        _myProvidersPool = new PoolingSystem<Utils.Provider>(myProvidersScrollPanel, myProviderItemPrefab, InitializeMyProviderItem, 10, false);
        _otherProvidersPool = new PoolingSystem<Utils.Provider>(otherProvidersScrollPanel, otherProviderItemPrefab, InitializeOtherProviderItem, 10);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetProvidersResponseEvent += OnGetProvidersResponse;
        EventManager.Instance.OnRemoveProviderResponseEvent += OnRemoveProviderResponse;
        EventManager.Instance.OnEditProviderResponseEvent += OnEditProviderResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetProvidersResponseEvent -= OnGetProvidersResponse;
        EventManager.Instance.OnRemoveProviderResponseEvent -= OnRemoveProviderResponse;
        EventManager.Instance.OnEditProviderResponseEvent -= OnEditProviderResponse;
    }

    public void OnGetProvidersResponse(GetProvidersResponse getProvidersResponse)
    {
        var myTeamProviders = getProvidersResponse.myTeamProviders;
        var otherTeamsProviders = getProvidersResponse.otherTeamsProviders;

        myTeamProviders.Reverse();
        otherTeamsProviders.Reverse();

        _myTeamProviderItemControllers.Clear();
        _otherTeamsProviderItemControllers.Clear();
        
        _myProvidersPool.RemoveAll();
        _otherProvidersPool.RemoveAll();

        for (int i=0;i < myTeamProviders.Count; i++)
        {
            if (myTeamProviders[i].state == Utils.ProviderState.TERMINATED) continue;
            
            _myProvidersPool.Add(myTeamProviders[i]);
        }
        for (int i = 0; i < otherTeamsProviders.Count; i++)
        {
            if (otherTeamsProviders[i].state == Utils.ProviderState.TERMINATED) continue;
            
            _otherProvidersPool.Add(otherTeamsProviders[i]);
        }
        
        RebuildListLayout(myProvidersScrollPanel);
        RebuildListLayout(otherProvidersScrollPanel);
    }

    private void OnRemoveProviderResponse(RemoveProviderResponse response)
    {
        if (!response.result.ToLower().Contains("provider removed"))
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }
        
        for (int i = 0; i < _myTeamProviderItemControllers.Count; i++)
        {
            var controller = _myTeamProviderItemControllers[i];

            if (controller.Provider.id != response.removedProviderId) continue;
            
            _myProvidersPool.Remove(controller.gameObject);
            _myTeamProviderItemControllers.Remove(controller);
            RebuildListLayout(myProvidersScrollPanel);
            return;
        }
    }

    public void OnEditProviderResponse(EditProviderResponse editProviderResponse)
    {
        if (editProviderResponse.editedProvider != null)
        {
            foreach (var controller in _myTeamProviderItemControllers)
            {
                if (controller.Provider.id != editProviderResponse.editedProvider.id) continue;
                
                controller.UpdateEditedProvider(editProviderResponse.editedProvider);
            }
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
        
        EditProviderPopupController.Instance.ClosePopup();
    }
    
    private void RebuildListLayout(RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void OnRefreshButtonClick()
    {
        GetProvidersRequest getProvidersRequest = new GetProvidersRequest(RequestTypeConstant.GET_PROVIDERS);
        RequestManager.Instance.SendRequest(getProvidersRequest);
    }

    private void InitializeMyProviderItem(GameObject theGameObject, int index, Utils.Provider provider)
    {
        var controller = theGameObject.GetComponent<MyProviderItemController>();
        controller.Initialize(provider);
        _myTeamProviderItemControllers.Add(controller);
    }
    
    private void InitializeOtherProviderItem(GameObject theGameObject, int index, Utils.Provider provider)
    {
        var controller = theGameObject.GetComponent<OtherProviderItemController>();
        controller.Initialize(provider);
        _otherTeamsProviderItemControllers.Add(controller);
    }

    public void AddMyProviderToList(Utils.Provider provider)
    {
        _myProvidersPool.Add(provider);
        RebuildListLayout(myProvidersScrollPanel);
    }

    public bool IsActiveProviderOfProductAtStorage(int productId, int storageId)
    {
        foreach (var controller in _myTeamProviderItemControllers)
        {
            if (controller.Provider.productId == productId && controller.Provider.storageId == storageId)
            {
                return true;
            }
        }

        return false;
    }
    
    public Tuple<float, float, float> CalculateMeanMaxMinByProductId(int productId)
    {
        float mean = 0, min = float.MaxValue, max = float.MinValue;
        int i = 0;

        foreach (var controller in _otherTeamsProviderItemControllers)
        {
            var provider = controller.Provider;
            
            if (provider.productId != productId) continue;
            if (provider.state == Utils.ProviderState.TERMINATED) continue;
        
            mean = (mean * i + provider.price) / (i + 1);
            if (provider.price > max)
            {
                max = provider.price;
            } else if (provider.price < min)
            {
                min = provider.price;
            }
        
            i++;
        }

        max = max == float.MinValue ? 0 : max;
        min = min == float.MaxValue ? 0 : min;

        return new Tuple<float, float, float>(mean, max, min);
    }
}
