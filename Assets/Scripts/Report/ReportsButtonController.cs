using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportsButtonController : MonoBehaviour
{
    public int index;

    public void OnChartButtonClick()
    {
        WeeklyReportManager.Instance.DrawChart(index);
    }
}
