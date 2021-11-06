using System.Linq;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ProductionLineTableRow : MonoBehaviour
    {
        #region UI

        public RTLTextMeshPro id_T;
        public RTLTextMeshPro name_T;
        public RTLTextMeshPro product_T;
        public RTLTextMeshPro amount_T;
        public RTLTextMeshPro endDate_T;
        public RTLTextMeshPro efficiency_T;
        public RTLTextMeshPro quality_T;
        public RTLTextMeshPro status_T;

        public Button showDetailButton;

        #endregion

        public Utils.ProductionLineDto Data { get; set; }


        public void SetData(Utils.ProductionLineDto data, bool justCreated = false)
        {
            Data = data;
            id_T.text = Data.id.ToString();
            name_T.text = GameDataManager.Instance.ProductionLineTemplates
                .FirstOrDefault(c => c.id == Data.productionLineTemplateId)?.name;
            if (!justCreated)
            {
                if (Data.products.Count > 0)
                {
                    product_T.text = GameDataManager.Instance.Products.FirstOrDefault(c => c.id == Data.products[0].id)
                        ?.name;
                    amount_T.text = Data.products[0].amount.ToString();
                    endDate_T.text = Data.products[0].endDate.ToString();
                }
                else
                {
                    product_T.text = "-";
                    amount_T.text = "-";
                    endDate_T.text = "-";
                }
            }
            else
            {
                product_T.text = "-";
                amount_T.text = "-";
                endDate_T.text = "-";
            }

            efficiency_T.text = ((EfficiencyLevel) Data.efficiencyLevel).ToString();
            quality_T.text = ((QualityLevel) Data.qualityLevel).ToString();
            status_T.text = Data.status.ToString();

            showDetailButton.onClick.AddListener(() => { ProductionLinesController.Instance.ShowDetails(Data.id); });
        }
    }
}