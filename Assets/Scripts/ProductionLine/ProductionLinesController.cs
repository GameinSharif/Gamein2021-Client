using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesController : MonoBehaviour
    {
        public static ProductionLinesController Instance;

        public GameObject productionLineRowPrefab;
        public Transform tableParent;

        public ProductionLineDetail productionLineDetail;
        public Transform constructButton;
        public GameObject constructPopup;

        private int currentDetailId = -1;
        private List<ProductionLineTableRow> productionLineTableRows = new List<ProductionLineTableRow>();

        private void Awake()
        {
            Instance = this;
            EventManager.Instance.OnGetProductionLinesResponseEvent += OnGetProductionLinesResponse;
            EventManager.Instance.OnConstructProductionLineResponseEvent += OnConstructProductionLineResponse;
            EventManager.Instance.OnScrapProductionLineResponseEvent += OnScrapProductionLineResponse;
            EventManager.Instance.OnStartProductionResponseEvent += OnStartProductionResponse;
            EventManager.Instance.OnUpgradeProductionLineEfficiencyResponseEvent +=
                OnUpgradeProductionLineEfficiencyResponse;
            EventManager.Instance.OnUpgradeProductionLineQualityResponseEvent += OnUpgradeProductionLineQualityResponse;
            productionLineDetail.gameObject.SetActive(false);
        }

        public void ConstructProductionLine(int productionLineTemplateId)
        {
            //TODO: check money
            
            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request = new ConstructProductionLineRequest(RequestTypeConstant.CONSTRUCT_PRODUCTION_LINE,
                        productionLineTemplateId);
                    RequestManager.Instance.SendRequest(request);
                }
            });
        }

        public void ShowDetails(int id)
        {
            var productionLine = productionLineTableRows.FirstOrDefault(e => e.Data.id == id);
            if (productionLine is null)
            {
                currentDetailId = -1;
                productionLineDetail.gameObject.SetActive(false);
                return;
            }

            currentDetailId = id;
            productionLineDetail.gameObject.SetActive(true);
            productionLineDetail.SetData(productionLine.Data);
        }

        private void UpdateDetails(Utils.ProductionLineDto data)
        {
            if (currentDetailId == data.id)
            {
                productionLineDetail.SetData(data);
            }
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < productionLineTableRows.Count; i++)
            {
                productionLineTableRows[i].SetRowNumber(i + 1);
            }

            constructButton.SetAsLastSibling();
        }
        
        public void PopupConstructProductionLine()
        {
            constructPopup.SetActive(true);
        }

        #region response events functions

        private void OnGetProductionLinesResponse(GetProductionLinesResponse response)
        {
            foreach (var item in response.productionLines
                    .Where(c => c.status == ProductionLineStatus.ACTIVE))
            {
                if (item.status == ProductionLineStatus.SCRAPPED) continue;
                var current = Instantiate(productionLineRowPrefab, tableParent).GetComponent<ProductionLineTableRow>();
                current.SetData(item);
                productionLineTableRows.Add(current);
            }

            UpdateRowNumbers();
        }

        private void OnConstructProductionLineResponse(ConstructProductionLineResponse response)
        {
            if (productionLineTableRows.Select(c => c.Data).Contains(response.productionLine)) return;
            var current = Instantiate(productionLineRowPrefab, tableParent).GetComponent<ProductionLineTableRow>();
            current.SetData(response.productionLine, true);
            productionLineTableRows.Add(current);
            UpdateRowNumbers();
        }

        private void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            if (current is null) return;
            productionLineTableRows.Remove(current);
            Destroy(current.gameObject);
            UpdateRowNumbers();

            ShowDetails(-1);
        }

        private void OnStartProductionResponse(StartProductionResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        #endregion
    }
}