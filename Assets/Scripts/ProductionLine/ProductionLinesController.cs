using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesController : MonoBehaviour
    {
        public GameObject productionLineCardPrefab;
        public Transform cardsParent;
        
        private List<Utils.ProductionLine> productionLines;

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
            var request = new ConstructProductionLineRequest(RequestTypeConstant.CONSTRUCT_PRODUCTION_LINE,
                productionLineTemplateId);
            RequestManager.Instance.SendRequest(request);
        }

        #region response events functions

        private void OnGetProductionLinesResponse(GetProductionLinesResponse response)
        {
            
        }
        private void OnConstructProductionLineResponse(ConstructProductionLineResponse response)
        {
            
        }
        private void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
        {
            
        }
        private void OnStartProductionResponse(StartProductionResponse response)
        {
            
        }
        private void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
        {
            
        }
        private void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
        {
            
        }
        
        #endregion
    }
}