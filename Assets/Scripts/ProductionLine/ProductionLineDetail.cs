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
            startProductionPopup.SetActive(true);
            startProductionPopup.GetComponent<StartProductionPopup>().Setup(Data);
        }

        public void UpgradeProductionLineEfficiency()
        {
            //TODO: check money
            
            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new UpgradeProductionLineEfficiencyRequest(RequestTypeConstant.UPGRADE_PRODUCTION_LINE_EFFICIENCY,
                            Data.id);
                    RequestManager.Instance.SendRequest(request); 
                }
            });
            
        }

        public void UpgradeProductionLineQuality()
        {
            //TODO: check money
            
            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new UpgradeProductionLineQualityRequest(RequestTypeConstant.UPGRADE_PRODUCTION_LINE_QUALITY, Data.id);
                    RequestManager.Instance.SendRequest(request);
                }
            });
        }

        public void ScrapProductionLine()
        {
            DialogManager.Instance.ShowConfirmDialog(agreed => {
                if (agreed)
                {
                    var request = new ScarpProductionLineRequest(RequestTypeConstant.SCRAP_PRODUCTION_LINE, Data.id);
                    RequestManager.Instance.SendRequest(request);
                    //TODO: add money
                }
            });
        }
    }
}