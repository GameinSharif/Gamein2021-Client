using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class ProvidersController : MonoBehaviour
{
    [HideInInspector] List<Utils.Provider> MyTeamProviders;
    [HideInInspector] List<Utils.Provider> OtherTeamsProviders;

    public GameObject providerItemPrefab;

    public GameObject MyTeamProvidersScrollViewParent;
    public GameObject OtherTeamsProvidersScrollViewParent;

    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    private void Awake()
    {
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

    private void AddMyProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem();
        createdItem.transform.SetParent(MyTeamProvidersScrollViewParent.transform);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

        createdItem.SetActive(true);
    }

    private void AddOtherProviderToList(Utils.Provider provider, int index)
    {
        GameObject createdItem = GetItem();
        createdItem.transform.SetParent(OtherTeamsProvidersScrollViewParent.transform);
        createdItem.transform.SetSiblingIndex(index);

        ProviderItemController controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(index, provider);

        createdItem.SetActive(true);
    }

    private GameObject GetItem()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        return Instantiate(providerItemPrefab);
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
