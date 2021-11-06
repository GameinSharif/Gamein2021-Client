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

        #endregion

        #region fields

        public Utils.ProductionLineDto Data { get; set; }

        #endregion

        public void SetData(Utils.ProductionLineDto data)
        {
            Data = data;
            if (data.qualityLevel == 2)
                qualityUpgradeButton.interactable = false;
            else
                qualityUpgradeButton.interactable = true;

            if (data.efficiencyLevel == 2)
                efficiencyUpgradeButton.interactable = false;
            else
                efficiencyUpgradeButton.interactable = true;
            
            if (data.status == ProductionLineStatus.SCRAPPED)
            {
                startProductionButton.interactable = false;
                scrapButton.interactable = false;
            }
            else
            {
                startProductionButton.interactable = true;
                scrapButton.interactable = true;
            }
        }

        public void ScrapProductionLine()
        {
            var request = new ScarpProductionLineRequest(RequestTypeConstant.SCRAP_PRODUCTION_LINE, Data.id);
            RequestManager.Instance.SendRequest(request);
        }

        public void StartProduction(int productId, int amount)
        {
            //TODO: check money
            var request = new StartProductionRequest(RequestTypeConstant.START_PRODUCTION, Data.id, productId, amount);
            RequestManager.Instance.SendRequest(request);
        }

        public void UpgradeProductionLineEfficiency()
        {
            //TODO: check money
            var request =
                new UpgradeProductionLineEfficiencyRequest(RequestTypeConstant.UPGRADE_PRODUCTION_LINE_EFFICIENCY,
                    Data.id);
            RequestManager.Instance.SendRequest(request);
        }

        public void UpgradeProductionLineQuality()
        {
            //TODO: check money
            var request =
                new UpgradeProductionLineQualityRequest(RequestTypeConstant.UPGRADE_PRODUCTION_LINE_QUALITY, Data.id);
            RequestManager.Instance.SendRequest(request);
        }
    }
}