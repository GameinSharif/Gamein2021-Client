using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro teamName;
    public RTLTextMeshPro value;
    public Image bg;

    public void SetInfo(Utils.Ranking ranking, int no, Sprite bg)
    {
        this.no.text = no.ToString();
        teamName.text = GameDataManager.Instance.GetTeamName(ranking.teamId);
        value.text = ranking.value.ToString("0.00");
        this.bg.sprite = bg;
    }
}
