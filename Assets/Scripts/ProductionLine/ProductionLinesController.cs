using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesController : MonoBehaviour
    {
        public List<Utils.ProductionLine> productionLines;
        
        private void Awake()
        {
            GetProductionLines();
        }

        private static void GetProductionLines()
        {
            var request = new GetProductionLinesRequest(RequestTypeConstant.GET_PRODUCTION_LINES);
            RequestManager.Instance.SendRequest(request);
        }
        
        public void ConstructProductionLine(int productionLineTemplateId)
        {
            var request = new ConstructProductionLineRequest(RequestTypeConstant.CONSTRUCT_PRODUCTION_LINE,
                productionLineTemplateId);
            RequestManager.Instance.SendRequest(request);
        }
    }
}