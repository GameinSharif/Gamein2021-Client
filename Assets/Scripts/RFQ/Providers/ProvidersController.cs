using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class ProvidersController : MonoBehaviour
{
    public static ProvidersController Instance;

    public GameObject providerItemPrefab;

    public GameObject MyTeamProvidersScrollViewParent;
    public GameObject OtherTeamsProvidersScrollViewParent;

    private List<ProviderItemController> _myTeamProviderItemControllers = new List<ProviderItemController>();
    private List<ProviderItemController> _otherTeamsProviderItemControllers = new List<ProviderItemController>();
    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        DeactiveAllChildrenInScrollPanel();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetProvidersResponseEvent += OnGetProvidersResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetProvidersResponseEvent -= OnGetProvidersResponse;
    }

    public void OnGetProvidersResponse(GetProvidersResponse getProvidersResponse)
    {
        List<Utils.Provider> myTeamProviders = getProvidersResponse.myTeamProviders;
        List<Utils.Provider> otherTeamsProviders = getProvidersResponse.otherTeamsProviders;

        myTeamProviders.Reverse();
        otherTeamsProviders.Reverse();

        _myTeamProviderItemControllers.Clear();
        _otherTeamsProviderItemControllers.Clear();
        DeactiveAllChildrenInScrollPanel();

        for (int i=0;i < myTeamProviders.Count; i++)
        {
            AddMyProviderToList(myTeamProviders[i], i + 1);
        }
        for (int i = 0; i < otherTeamsProviders.Count; i++)
        {
            AddOtherProviderToList(otherTeamsProviders[i], i + 1);
        }
    }

    public void AddMyProviderToList(Utils.Provider provider)
    {
        AddMyProviderToList(provider, _myTeamProviderItemControllers.Count);
    }

    private void AddMyProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem(MyTeamProvidersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

        _myTeamProviderItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private void AddOtherProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem(OtherTeamsProvidersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

        _otherTeamsProviderItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(providerItemPrefab, parent.transform);
        _spawnedGameObjects.Add(newItem);
        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnRefreshButtonClick()
    {
        GetProvidersRequest getProvidersRequest = new GetProvidersRequest(RequestTypeConstant.GET_PROVIDERS);
        RequestManager.Instance.SendRequest(getProvidersRequest);
    }
}
