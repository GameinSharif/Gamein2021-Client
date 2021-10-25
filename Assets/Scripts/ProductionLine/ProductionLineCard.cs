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

        
        private Utils.ProductionLine data;

        #endregion

        public void SetData(Utils.ProductionLine data)
        {
            this.data = data;
        }
    }
}