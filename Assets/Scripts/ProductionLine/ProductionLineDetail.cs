using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ProductionLineDetail : MonoBehaviour
    {
        #region UI

        public Button startProductionButton;
        public Button qualityUpgradeButton;
        public Button efficiencyUpgradeButton;
        public Button scrapButton;

        public GameObject startProductionPopup;

        public GameObject inProcessPanel;
        public Localize remainingTime_T;

        #endregion

        #region fields

        public Utils.ProductionLineDto Data { get; set; }

        #endregion

        private void Awake()
        {
            startProductionPopup.SetActive(false);
        }

        public void SetData(Utils.ProductionLineDto data)
        {
            Data = data;

            qualityUpgradeButton.interactable = data.status == ProductionLineStatus.ACTIVE && data.qualityLevel != 2;

            efficiencyUpgradeButton.interactable =
                data.status == ProductionLineStatus.ACTIVE && data.efficiencyLevel != 2;

            //startProductionButton.interactable = data.status == ProductionLineStatus.ACTIVE;
            scrapButton.interactable = data.status == ProductionLineStatus.ACTIVE;

            inProcessPanel.SetActive(false);
            
            if (data.status == ProductionLineStatus.IN_CONSTRUCTION)
            {
                var remainingTime = (data.activationDate.ToDateTime() -
                                 MainHeaderManager.Instance.gameDate.ToDateTime()).Days;
                remainingTime_T.SetKey("construction_remaining_time", remainingTime.ToString());
                inProcessPanel.SetActive(true);
                return;
            }
            
            if (data.products is null) return;
            if (data.products.Count == 0) return;
            var remaining = (data.products.Last().endDate.ToDateTime() -
                            MainHeaderManager.Instance.gameDate.ToDateTime()).Days;
            if (remaining <= 0) return;

            remainingTime_T.SetKey("production_remaining_time", remaining.ToString());
            inProcessPanel.SetActive(true);
        }


        public void PopupStartProduction()
        {
            startProductionPopup.SetActive(true);
            startProductionPopup.GetComponent<StartProductionPopup>().Setup(Data);
        }

        public void UpgradeProductionLineEfficiency()
        {
            if (GameDataManager.Instance.GetProductionLineTemplateById(Data.productionLineTemplateId)
                    .efficiencyLevels[Data.efficiencyLevel + 1].upgradeCost >
                MainHeaderManager.Instance.Money)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new UpgradeProductionLineEfficiencyRequest(
                            RequestTypeConstant.UPGRADE_PRODUCTION_LINE_EFFICIENCY,
                            Data.id);
                    RequestManager.Instance.SendRequest(request);
                }
            });
        }

        public void UpgradeProductionLineQuality()
        {
            if (GameDataManager.Instance.GetProductionLineTemplateById(Data.productionLineTemplateId)
                    .qualityLevels[Data.qualityLevel + 1].upgradeCost >
                MainHeaderManager.Instance.Money)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new UpgradeProductionLineQualityRequest(RequestTypeConstant.UPGRADE_PRODUCTION_LINE_QUALITY,
                            Data.id);
                    RequestManager.Instance.SendRequest(request);
                }
            });
        }

        public void ScrapProductionLine()
        {
            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request = new ScarpProductionLineRequest(RequestTypeConstant.SCRAP_PRODUCTION_LINE, Data.id);
                    RequestManager.Instance.SendRequest(request);
                }
            });
        }
    }
}