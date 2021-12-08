using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject leaderboardCanvas;

    public GameObject rankItemPrefab;
    public GameObject leaderboardScrollViewParent;
    public List<Sprite> rankBg;
    public Localize yourTeamLocalize;
    public RTLTextMeshPro yourNo;
    public RTLTextMeshPro yourValue;

    private List<GameObject> _spawnedRankGameObjects = new List<GameObject>();
    
    public Localize totalTeams;
    
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
            AddFinishedProductItemToList(rankings[i], i + 1);
        }
        yourTeamLocalize.SetKey("leaderboard_your_team");
        yourNo.text = getLeaderboardResponse.yourRanking.ToString();
        yourValue.text = MainHeaderManager.Instance.Value.ToString("0.00");
        totalTeams.SetKey("leaderboard_total_teams", getLeaderboardResponse.totalTeams.ToString());
        leaderboardCanvas.SetActive(true);
    }
    
    private void AddFinishedProductItemToList(Utils.Ranking ranking, int index)
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
            leaderboardCanvas.SetActive(false);
            return;
        }
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest(RequestTypeConstant.GET_LEADERBOARD);
        RequestManager.Instance.SendRequest(getLeaderboardRequest);
    }
}
