using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AwesomeCharts;

public class WeeklyReportManager: MonoBehaviour
{
    public static WeeklyReportManager Instance;

    public LineChart RankingLineChart;
    public LineChart BrandLineChart;
    public LineChart FinanceLineChart;
    public LineChart CostsLineChart;
    public LineChart InventoryLineChart;

    [HideInInspector] public List<Utils.WeeklyReport> WeeklyReports;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetAllWeeklyReportsResponseEvent += OnGetAllWeeklyReportsResponseReceived;
        EventManager.Instance.OnUpdateWeeklyReportResponseEvent += OnUpdateWeeklyReportResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetAllWeeklyReportsResponseEvent -= OnGetAllWeeklyReportsResponseReceived;
        EventManager.Instance.OnUpdateWeeklyReportResponseEvent -= OnUpdateWeeklyReportResponseReceived;
    }

    private void OnGetAllWeeklyReportsResponseReceived(GetAllWeeklyReportsResponse getAllWeeklyReportsResponse)
    {
        WeeklyReports = getAllWeeklyReportsResponse.weeklyReports;
    }

    private void OnUpdateWeeklyReportResponseReceived(UpdateWeeklyReportResponse updateWeeklyReportResponse)
    {
        WeeklyReports.Add(updateWeeklyReportResponse.weeklyReport);
    }

    public void DrawCharts()
    {
        DrawRankingChart();
    }

    private void DrawRankingChart()
    {
        List<LineEntry> lineEntries = new List<LineEntry>();
        foreach (Utils.WeeklyReport weeklyReport in WeeklyReports)
        {
            LineEntry lineEntry = new LineEntry(weeklyReport.weekNumber, weeklyReport.ranking);
            lineEntries.Add(lineEntry);
        }

        RankingLineChart.data.DataSets[0].Entries = lineEntries;
    }
}
