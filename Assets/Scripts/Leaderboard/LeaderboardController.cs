using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public Transform leaderboardPanelTransform;
    
    public GameObject rankItemPrefab;
    public GameObject leaderboardScrollViewParent;
    public List<Sprite> rankBg;
    public Localize yourTeamLocalize;
    public RTLTextMeshPro yourNo;
    public RTLTextMeshPro yourValue;

    public List<GameObject> _spawnedRankGameObjects;
    
    public Localize totalTeams;

    public float closedPosY, openedPosY;
    
    private void OnEnable()
    {
        EventManager.Instance.OnGetLeaderboardResponseEvent += OnGetLeaderboardResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetLeaderboardResponseEvent -= OnGetLeaderboardResponse;
    }

    private void OnGetLeaderboardResponse(GetLeaderboardResponse getLeaderboardResponse)
    {
        List<Utils.Ranking> rankings = getLeaderboardResponse.rankings;
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < rankings.Count; i++)
        {
            AddRankItemToList(rankings[i], i + 1);
        }

        yourTeamLocalize.SetKey("leaderboard_your_team");
        yourNo.text = getLeaderboardResponse.yourRanking.ToString();
        yourValue.text = getLeaderboardResponse.yourWealth.ToString("0.00");
        totalTeams.SetKey("leaderboard_total_teams", GameDataManager.Instance.Teams.Count.ToString());

        leaderboardCanvas.SetActive(true);
        leaderboardPanelTransform.DOMoveY(openedPosY, 0.5f).SetEase(Ease.Linear);
    }
    
    private void AddRankItemToList(Utils.Ranking ranking, int index)
    {
        GameObject createdItem = GetItem(leaderboardScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        RankItemController controller = createdItem.GetComponent<RankItemController>();
        int spriteIndex = index - 4 < 0 ? index - 1 : 3;
        controller.SetInfo(ranking, index, rankBg[spriteIndex]);
        createdItem.SetActive(true);
    }
    
    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedRankGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(rankItemPrefab, parent.transform);
        _spawnedRankGameObjects.Add(newItem);

        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedRankGameObjects)
        {
            gameObject.SetActive(false);
        }
    }    
    
    public void OnLeaderboardButtonClicked()
    {
        if (leaderboardCanvas.activeSelf)
        {
            leaderboardPanelTransform.DOMoveY(closedPosY, 0.5f).SetEase(Ease.Linear).onComplete += () => leaderboardCanvas.SetActive(false);;
            return;
        }

        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest(RequestTypeConstant.GET_LEADERBOARD);
        RequestManager.Instance.SendRequest(getLeaderboardRequest);
    }
}
