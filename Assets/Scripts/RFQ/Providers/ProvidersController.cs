using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class ProvidersController : MonoBehaviour
{
    public static ProvidersController Instance;

    [HideInInspector] List<Utils.Provider> MyTeamProviders;
    [HideInInspector] List<Utils.Provider> OtherTeamsProviders;

    public GameObject providerItemPrefab;

    public GameObject MyTeamProvidersScrollViewParent;
    public GameObject OtherTeamsProvidersScrollViewParent;

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
        MyTeamProviders = getProvidersResponse.myTeamProviders;
        OtherTeamsProviders = getProvidersResponse.otherTeamsProviders;

        MyTeamProviders.Reverse();
        OtherTeamsProviders.Reverse();

        DeactiveAllChildrenInScrollPanel();
        for (int i=0;i < MyTeamProviders.Count; i++)
        {
            AddMyProviderToList(MyTeamProviders[i], i + 1);
        }
        for (int i = 0; i < OtherTeamsProviders.Count; i++)
        {
            AddOtherProviderToList(OtherTeamsProviders[i], i + 1);
        }
    }

    public void AddMyProviderToList(Utils.Provider provider)
    {
        MyTeamProviders.Add(provider);
        AddMyProviderToList(provider, MyTeamProviders.Count);
    }

    private void AddMyProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem(MyTeamProvidersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

        createdItem.SetActive(true);
    }

    private void AddOtherProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem(OtherTeamsProvidersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

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
