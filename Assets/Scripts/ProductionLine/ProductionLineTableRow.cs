using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ProductionLineTableRow : MonoBehaviour
    {
        #region UI

        //public RTLTextMeshPro number_T;
        public Localize name_T;
        //public Localize product_L;
        //public RTLTextMeshPro product_T;
        //public RTLTextMeshPro amount_T;
        //public RTLTextMeshPro endDate_T;
        //public Localize efficiency_L;
        //public Localize quality_L;
        //public RTLTextMeshPro status_T;
        public Localize status_L;

        public List<GameObject> qualityStars = new List<GameObject>();
        public List<GameObject> efficiencyStars = new List<GameObject>();

        public Button showDetailButton;
        private CanvasGroup panel;

        private bool isBusy = false;
        public CanvasGroup productionStatusPopup;
        public Image productIcon;
        public RTLTextMeshPro productAmount;
        public Localize endDate;
        
        #endregion

        public Utils.ProductionLineDto Data { get; set; }

        private void Awake()
        {
            panel = GetComponent<CanvasGroup>();
        }

        public void SetData(Utils.ProductionLineDto data, bool justCreated = false)
        {
            Data = data;
            name_T.SetKey("production_line_template_" + GameDataManager.Instance.ProductionLineTemplates
                .FirstOrDefault(c => c.id == Data.productionLineTemplateId)?.name);
            //efficiency_L.SetKey(((EfficiencyLevel) Data.efficiencyLevel).ToString());
            //quality_L.SetKey(((QualityLevel) Data.qualityLevel).ToString());
            for (int i = 0; i < qualityStars.Count; i++)
            {
                qualityStars[i].SetActive(i <= Data.qualityLevel);
            }

            for (int i = 0; i < efficiencyStars.Count; i++)
            {
                efficiencyStars[i].SetActive(i <= Data.efficiencyLevel);
            }

            showDetailButton.onClick.AddListener(() => { ProductionLinesController.Instance.ShowDetails(Data.id); });

            isBusy = false;
            if (Data.status == ProductionLineStatus.IN_CONSTRUCTION)
            {
                var remainingTime = (data.activationDate.ToDateTime() -
                                     MainHeaderManager.Instance.gameDate.ToDateTime()).Days;
                status_L.SetKey("time_DAY", remainingTime.ToString());
                //status_T.text = remainingTime + " D";
                showDetailButton.interactable = false;
            }
            else
            {
                if (Data.products.Count == 0)
                {
                    status_L.SetKey("ACTIVE");
                    showDetailButton.interactable = true;
                    return;
                }

                var currentProduction =
                    Data.products.Last().endDate.ToDateTime() > MainHeaderManager.Instance.gameDate.ToDateTime()
                        ? Data.products.Last()
                        : null;
                if (currentProduction is null)
                {
                    status_L.SetKey("ACTIVE");
                    showDetailButton.interactable = true;
                }
                else
                {
                    status_L.SetKey("BUSY");
                    showDetailButton.interactable = false;
                    isBusy = true;
                }
            }
        }

        public void Highlight(bool on)
        {
            panel.alpha = on ? 0.5f : 1;
            //showDetailButton.interactable = !on;
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

        public void ShowProductionStatusPopup()
        {
            if(!isBusy) return;
            var currentProduction = Data.products.Last();
            productIcon.sprite = GameDataManager.Instance.ProductSprites[currentProduction.productId];
            productAmount.text = currentProduction.amount.ToString();
            var endDateString = currentProduction.endDate.year+"/"+currentProduction.endDate.month+"/" +currentProduction.endDate.day;
            endDate.SetKey("production_line_end_date_show", endDateString);
            productionStatusPopup.DOFade(1, 0.2f);
        }
        
        public void HideProductionStatusPopup()
        {
            productionStatusPopup.DOFade(0, 0.2f);
        }
    }
}