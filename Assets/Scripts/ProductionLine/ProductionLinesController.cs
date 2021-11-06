using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesController : MonoBehaviour
    {
        public GameObject productionLineRowPrefab;
        public Transform cardsParent;
        
        //private List<Utils.ProductionLineDto> productionLines;
        private List<ProductionLineTableRow> productionLineTableRows = new List<ProductionLineTableRow>();

        private void Awake()
        {
            GetProductionLines();
            EventManager.Instance.OnGetProductionLinesResponseEvent += OnGetProductionLinesResponse;
            EventManager.Instance.OnConstructProductionLineResponseEvent += OnConstructProductionLineResponse;
            EventManager.Instance.OnScrapProductionLineResponseEvent += OnScrapProductionLineResponse;
            EventManager.Instance.OnStartProductionResponseEvent += OnStartProductionResponse;
            EventManager.Instance.OnUpgradeProductionLineEfficiencyResponseEvent += OnUpgradeProductionLineEfficiencyResponse;
            EventManager.Instance.OnUpgradeProductionLineQualityResponseEvent += OnUpgradeProductionLineQualityResponse;
        }
        
        private static void GetProductionLines()
        {
            var request = new GetProductionLinesRequest(RequestTypeConstant.GET_PRODUCTION_LINES);
            RequestManager.Instance.SendRequest(request);
        }
        
        public void ConstructProductionLine(int productionLineTemplateId)
        {
            //TODO: check money
            var request = new ConstructProductionLineRequest(RequestTypeConstant.CONSTRUCT_PRODUCTION_LINE,
                productionLineTemplateId);
            RequestManager.Instance.SendRequest(request);
        }

        #region response events functions

        private void OnGetProductionLinesResponse(GetProductionLinesResponse response)
        {
            foreach (var productionLineData in response.productionLines)
            {
                var current = Instantiate(productionLineRowPrefab, cardsParent).GetComponent<ProductionLineTableRow>();
                current.SetData(productionLineData);
                productionLineTableRows.Add(current);
            }
        }
        private void OnConstructProductionLineResponse(ConstructProductionLineResponse response)
        {
            if(productionLineTableRows.Select(c => c.Data).Contains( response.productionLine)) return;
            var current = Instantiate(productionLineRowPrefab, cardsParent).GetComponent<ProductionLineTableRow>();
            current.SetData(response.productionLine);
            productionLineTableRows.Add(current);
        }
        private void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            if(current is null) return;
            productionLineTableRows.Remove(current);
            Destroy(current.gameObject);
        }
        private void OnStartProductionResponse(StartProductionResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
        }
        private void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
        }
        private void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
        }
        
        #endregion
    }
}