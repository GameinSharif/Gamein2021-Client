using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GetLeaderboardResponse : ResponseObject
{
    public List<Utils.Ranking> rankings;
    public int yourRanking;
    public int totalTeams;
}
