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
        public TMP_Dropdown productId_I;
        public TMP_InputField productAmount_I;

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

            qualityUpgradeButton.interactable = data.qualityLevel != 2;

            efficiencyUpgradeButton.interactable = data.efficiencyLevel != 2;

            startProductionButton.interactable = data.status == ProductionLineStatus.ACTIVE;
            scrapButton.interactable = data.status == ProductionLineStatus.ACTIVE;

            if (data.products != null && data.products.Count > 0) startProductionButton.interactable = false;
        }


        public void PopupStartProduction()
        {
            productId_I.options.Clear();
            var products = GameDataManager.Instance.Products.Where(c => c.productionLineTemplateId == Data.productionLineTemplateId).ToList();
            foreach (var product in products)
            {
                productId_I.options.Add(new TMP_Dropdown.OptionData(product.id.ToString()));
            }
            startProductionPopup.SetActive(true);
        }

        public void ClosePopup()
        {
            startProductionPopup.SetActive(false);
        }

        public void StartProduction()
        {
            int productId = int.Parse(productId_I.options[productId_I.value].text);
            int amount = int.Parse(productAmount_I.text);
            //TODO: check money and materials
            var request = new StartProductionRequest(RequestTypeConstant.START_PRODUCTION, Data.id, productId, amount);
            RequestManager.Instance.SendRequest(request);
            startProductionPopup.SetActive(false);
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

        public void ScrapProductionLine()
        {
            var request = new ScarpProductionLineRequest(RequestTypeConstant.SCRAP_PRODUCTION_LINE, Data.id);
            RequestManager.Instance.SendRequest(request);
            //TODO: add money
        }
    }
}