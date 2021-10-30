using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesController : MonoBehaviour
    {
        public GameObject productionLineCardPrefab;
        public Transform cardsParent;
        
        //private List<Utils.ProductionLineDto> productionLines;
        private List<ProductionLineCard> productionLineCards = new List<ProductionLineCard>();

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
                var current = Instantiate(productionLineCardPrefab, cardsParent).GetComponent<ProductionLineCard>();
                current.SetData(productionLineData);
                productionLineCards.Add(current);
            }
        }
        private void OnConstructProductionLineResponse(ConstructProductionLineResponse response)
        {
            if(productionLineCards.Select(c => c.Data).Contains( response.productionLine)) return;
            var current = Instantiate(productionLineCardPrefab, cardsParent).GetComponent<ProductionLineCard>();
            current.SetData(response.productionLine);
            productionLineCards.Add(current);
        }
        private void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
        {
            var current = productionLineCards.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            if(current is null) return;
            productionLineCards.Remove(current);
            Destroy(current.gameObject);
        }
        private void OnStartProductionResponse(StartProductionResponse response)
        {
            var current = productionLineCards.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.UpdateData(response.productionLine);
        }
        private void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
        {
            var current = productionLineCards.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.UpdateData(response.productionLine);
        }
        private void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
        {
            var current = productionLineCards.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.UpdateData(response.productionLine);
        }
        
        #endregion
    }
}