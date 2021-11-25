using System;
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

        public Button showDetailButton;

        #endregion

        public Utils.ProductionLineDto Data { get; set; }


        public void SetData(Utils.ProductionLineDto data, bool justCreated = false)
        {
            Data = data;
            name_T.SetKey("production_line_template_" + GameDataManager.Instance.ProductionLineTemplates
                .FirstOrDefault(c => c.id == Data.productionLineTemplateId)?.name);
            efficiency_L.SetKey(((EfficiencyLevel) Data.efficiencyLevel).ToString());
            quality_L.SetKey(((QualityLevel) Data.qualityLevel).ToString());
            status_T.text = Data.status.ToString();

            showDetailButton.onClick.AddListener(() => { ProductionLinesController.Instance.ShowDetails(Data.id); });
            
            if (justCreated)
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
            endDate_T.text = currentProduction.endDate.ToString();
        }

        public void SetRowNumber(int number)
        {
            number_T.text = number.ToString();
        }

        public void Highlight(bool on)
        {
            showDetailButton.interactable = !on;
        }
    }
}