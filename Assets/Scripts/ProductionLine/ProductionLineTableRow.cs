using System;
using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ProductionLineTableRow : MonoBehaviour
    {
        #region UI

        public RTLTextMeshPro number_T;
        public Localize name_T;
        public Localize product_L;
        public RTLTextMeshPro product_T;
        public RTLTextMeshPro amount_T;
        public RTLTextMeshPro endDate_T;
        public Localize efficiency_L;
        public Localize quality_L;
        public RTLTextMeshPro status_T;
        public Localize status_L;

        public List<GameObject> qualityStars = new List<GameObject>();
        public List<GameObject> efficiencyStars = new List<GameObject>();

        public Button showDetailButton;
        private CanvasGroup panel;

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

            if (Data.status == ProductionLineStatus.IN_CONSTRUCTION)
            {
                var remainingTime = (data.activationDate.ToDateTime() -
                                     MainHeaderManager.Instance.gameDate.ToDateTime()).Days;
                status_L.SetKey("");
                status_T.text = remainingTime + " D";
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
                }
            }

            /*if (justCreated)
            {
                product_T.text = "-";
                amount_T.text = "-";
                endDate_T.text = "-";
                return;
            }
            if (Data.products.Count == 0)
            {
                product_T.text = "-";
                amount_T.text = "-";
                endDate_T.text = "-";
                return;
            }
            var currentProduction =
                Data.products.Last().endDate.ToDateTime() > MainHeaderManager.Instance.gameDate.ToDateTime()
                    ? Data.products.Last()
                    : null;
            if (currentProduction is null)
            {
                product_T.text = "-";
                amount_T.text = "-";
                endDate_T.text = "-";
                return;
            }
            product_L.SetKey("product_" + GameDataManager.Instance.Products
                .FirstOrDefault(c => c.id == currentProduction.productId)
                ?.name);
            amount_T.text = currentProduction.amount.ToString();
            endDate_T.text = currentProduction.endDate.ToString();*/
        }

        public void SetRowNumber(int number)
        {
            number_T.text = number.ToString();
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
    }
}