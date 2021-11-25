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
            EventManager.Instance.OnUpgradeProductionLineEfficiencyResponseEvent += OnUpgradeProductionLineEfficiencyResponse;
            EventManager.Instance.OnUpgradeProductionLineQualityResponseEvent += OnUpgradeProductionLineQualityResponse;
            EventManager.Instance.OnProductionLineConstructionCompletedResponseEvent += OnProductionLineConstructionCompletedResponse;
            EventManager.Instance.OnProductCreationCompletedResponseEvent += OnProductCreationCompletedResponse;

            productionLineDetail.gameObject.SetActive(false);
        }

        public void ConstructProductionLine(int productionLineTemplateId)
        {
            if (GameDataManager.Instance.GetProductionLineTemplateById(productionLineTemplateId).constructionCost > MainHeaderManager.Instance.Money)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

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
            currentDetailId = -1;
            foreach (var row in productionLineTableRows)
            {
                if (row.Data.id == id)
                {
                    currentDetailId = id;
                    row.Highlight(true);
                    productionLineDetail.gameObject.SetActive(true);
                    productionLineDetail.SetData(row.Data);
                }
                else
                {
                    row.Highlight(false);
                }
            }

            if (currentDetailId == -1)
            {
                productionLineDetail.gameObject.SetActive(false);
            }
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
            if (response.productionLines is null)
            {
                DialogManager.Instance.ShowErrorDialog();
                Debug.LogError("error while getting production lines");
                return;
            }

            foreach (var item in productionLineTableRows)
            {
                Destroy(item.gameObject);
            }

            productionLineTableRows.Clear();
            ProductionLinesDataManager.Instance.productionLineDtos =
                new List<Utils.ProductionLineDto>(response.productionLines); //saving a copy in data manager object
            
            foreach (var item in response.productionLines.Where(c => c.status != ProductionLineStatus.SCRAPPED))
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
            if (response.productionLine is null)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            MainHeaderManager.Instance.Money -= GameDataManager.Instance.GetProductionLineTemplateById(response.productionLine.productionLineTemplateId).constructionCost;
            
            if (productionLineTableRows.Select(c => c.Data).Contains(response.productionLine)) return;
            var current = Instantiate(productionLineRowPrefab, tableParent).GetComponent<ProductionLineTableRow>();
            current.SetData(response.productionLine, true);
            productionLineTableRows.Add(current);
            UpdateRowNumbers();
        }

        private void OnScrapProductionLineResponse(ScrapProductionLineResponse response)
        {
            if (response.productionLine is null)
            {
                Debug.LogError("cant scrap production line");
                return;
            }

            MainHeaderManager.Instance.Money += GameDataManager.Instance.GetProductionLineTemplateById(response.productionLine.productionLineTemplateId).scrapPrice;
            
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            if (current is null) return;
            productionLineTableRows.Remove(current);
            Destroy(current.gameObject);
            UpdateRowNumbers();

            ShowDetails(-1);
        }

        private void OnStartProductionResponse(StartProductionResponse response)
        {
            if (response.productionLine is null)
            {
                Debug.LogError("cant start production");
                return;
            }
            
            var template = GameDataManager.Instance.GetProductionLineTemplateById(response.productionLine.productionLineTemplateId);
            var production = response.productionLine.products.Last();
           
            MainHeaderManager.Instance.Money -= template.productionCostPerOneProduct * production.amount + template.setupCost;
            
            var ingredients = GameDataManager.Instance.GetProductById(production.productId).ingredientsPerUnit;
            if (ingredients != null)
            {
                foreach (var ingredient in ingredients)
                {
                    StorageManager.Instance.ChangeStockInStorage(StorageManager.Instance.GetWarehouse().id,
                        ingredient.productId, - ingredient.amount * production.amount);
                }
            }


            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnUpgradeProductionLineEfficiencyResponse(UpgradeProductionLineEfficiencyResponse response)
        {
            if (response.productionLine is null)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            MainHeaderManager.Instance.Money -= GameDataManager.Instance
                .GetProductionLineTemplateById(response.productionLine.productionLineTemplateId)
                .efficiencyLevels[response.productionLine.efficiencyLevel].upgradeCost;
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnUpgradeProductionLineQualityResponse(UpgradeProductionLineQualityResponse response)
        {
            if (response.productionLine is null)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            MainHeaderManager.Instance.Money -= GameDataManager.Instance
                .GetProductionLineTemplateById(response.productionLine.productionLineTemplateId)
                .qualityLevels[response.productionLine.qualityLevel].upgradeCost;
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnProductionLineConstructionCompletedResponse(ProductionLineConstructionCompletedResponse response)
        {
            var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productionLine.id);
            current?.SetData(response.productionLine);
            UpdateDetails(response.productionLine);
        }

        private void OnProductCreationCompletedResponse(ProductCreationCompletedResponse response)
        {
            //TODO
            //var current = productionLineTableRows.FirstOrDefault(e => e.Data.id == response.productLineId);
            //current?.SetData(response.productionLine);
            //UpdateDetails(response.productionLine);

            StorageManager.Instance.ChangeStockInStorage(StorageManager.Instance.GetWarehouse().id,
                response.product.productId, response.product.amount);
        }

        #endregion
    }
}
