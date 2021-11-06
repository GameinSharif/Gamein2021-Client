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

        #endregion
        
        public Utils.ProductionLineDto Data { get; set; }
        
        public void SetData(Utils.ProductionLineDto data)
        {
            this.Data = data;
            id_T.SetText(Data.id.ToString());
            name_T.SetText(GameDataManager.Instance.ProductionLineTemplates.FirstOrDefault(c => c.id==Data.productionLineTemplateId)?.name);
            product_T.SetText(GameDataManager.Instance.Products.FirstOrDefault(c => c.id == Data.products[0].id)?.name);
            amount_T.SetText(Data.products[0].amount.ToString());
            endDate_T.SetText(Data.products[0].endDate.ToString());
            efficiency_T.SetText(Data.efficiencyLevel.ToString());
            quality_T.SetText(Data.qualityLevel.ToString());
            status_T.SetText(Data.status.ToString());
        }
    }
}