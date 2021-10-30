using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ProductionLineCard : MonoBehaviour
    {
        #region UI

        public Button scrapButton;
        public Image productionLineAvatar;

        #endregion

        #region fields

        public Utils.ProductionLineDto Data { get; set; }

        #endregion

        public void SetData(Utils.ProductionLineDto data)
        {
            this.Data = data;
        }

        public void UpdateData(Utils.ProductionLineDto data)
        {
            this.Data = data;
            //TODO: update card            
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