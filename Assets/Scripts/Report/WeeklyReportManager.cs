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

    private int _index = 0;

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
        OnOpenReportsPage();
    }

    public void OnOpenReportsPage()
    {
        DrawChart(_index);
    }

    public void DrawChart(int index)
    {
        DisableAll();

        switch(index)
        {
            case 0:
                DrawRankingChart();
                RankingLineChart.gameObject.SetActive(true);
                break;
            case 1:
                DrawBrandChart();
                BrandLineChart.gameObject.SetActive(true);
                break;
            case 2:
                DrawFinanceChart();
                FinanceLineChart.gameObject.SetActive(true);
                break;
            case 3:
                DrawCostsChart();
                CostsLineChart.gameObject.SetActive(true);
                break;
            case 4:
                DrawInventoryChart();
                InventoryLineChart.gameObject.SetActive(true);
                break;
        }

        _index = index;
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
        RankingLineChart.SetDirty();
        RankingLineChart.enabled = false;
        RankingLineChart.enabled = true;
    }

    private void DrawBrandChart()
    {
        List<LineEntry> lineEntries = new List<LineEntry>();
        foreach (Utils.WeeklyReport weeklyReport in WeeklyReports)
        {
            LineEntry lineEntry = new LineEntry(weeklyReport.weekNumber, weeklyReport.brand);
            lineEntries.Add(lineEntry);
        }

        BrandLineChart.data.DataSets[0].Entries = lineEntries;
        BrandLineChart.SetDirty();
        BrandLineChart.enabled = false;
        BrandLineChart.enabled = true;
    }

    private void DrawFinanceChart()
    {
        List<LineEntry> totalCapitalLineEntries = new List<LineEntry>();
        List<LineEntry> inFlowLineEntries = new List<LineEntry>();
        List<LineEntry> outFlowLineEntries = new List<LineEntry>();

        for(int i=0;i < WeeklyReports.Count; i++)
        {
            Utils.WeeklyReport weeklyReport = WeeklyReports[i];

            totalCapitalLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.totalCapital));

            inFlowLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.inFlow));
            outFlowLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.outFlow));
        }

        FinanceLineChart.data.DataSets[0].Entries = totalCapitalLineEntries;
        FinanceLineChart.data.DataSets[1].Entries = inFlowLineEntries;
        FinanceLineChart.data.DataSets[2].Entries = outFlowLineEntries;
        FinanceLineChart.SetDirty();

        FinanceLineChart.enabled = false;
        FinanceLineChart.enabled = true;
    }

    private void DrawCostsChart()
    {
        List<LineEntry> transportCostsLineEntries = new List<LineEntry>();
        List<LineEntry> productionCostsLineEntries = new List<LineEntry>();

        for (int i = 0; i < WeeklyReports.Count; i++)
        {
            Utils.WeeklyReport weeklyReport = WeeklyReports[i];

            transportCostsLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.transportationCosts));
            productionCostsLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.productionCosts));
        }

        CostsLineChart.data.DataSets[0].Entries = transportCostsLineEntries;
        CostsLineChart.data.DataSets[1].Entries = productionCostsLineEntries;
        CostsLineChart.SetDirty();

        CostsLineChart.enabled = false;
        CostsLineChart.enabled = true;
    }

    private void DrawInventoryChart()
    {
        List<LineEntry> rawMaterialsLineEntries = new List<LineEntry>();
        List<LineEntry> semiFinishedLineEntries = new List<LineEntry>();
        List<LineEntry> finishedEntries = new List<LineEntry>();

        for (int i = 0; i < WeeklyReports.Count; i++)
        {
            Utils.WeeklyReport weeklyReport = WeeklyReports[i];

            rawMaterialsLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.rawMaterialPercentage));
            semiFinishedLineEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.intermediateMaterialPercentage));
            finishedEntries.Add(new LineEntry(weeklyReport.weekNumber, weeklyReport.finalProductPercentage));
        }

        InventoryLineChart.data.DataSets[0].Entries = rawMaterialsLineEntries;
        InventoryLineChart.data.DataSets[1].Entries = semiFinishedLineEntries;
        InventoryLineChart.data.DataSets[2].Entries = finishedEntries;
        InventoryLineChart.SetDirty();

        InventoryLineChart.enabled = false;
        InventoryLineChart.enabled = true;
    }

    private void DisableAll()
    {
        RankingLineChart.gameObject.SetActive(false);
        BrandLineChart.gameObject.SetActive(false);
        FinanceLineChart.gameObject.SetActive(false);
        CostsLineChart.gameObject.SetActive(false);
        InventoryLineChart.gameObject.SetActive(false);
    }
}
