using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionLine
{
    public class ProductionLinesDataManager : MonoBehaviour
    {
        public static ProductionLinesDataManager Instance;

        public List<Utils.ProductionLineDto> productionLineDtos;

        private void Awake()
        {
            Instance = this;
        }

        public bool HasProductionLineOfProduct(Utils.Product product)
        {
            return productionLineDtos.Exists(pl =>
                   (pl.status != ProductionLineStatus.SCRAPPED)
                && (pl.productionLineTemplateId == product.productionLineTemplateId));
        }

        public bool CanUseProduct(Utils.Product product)
        {
            if (product.categoryIds == null)
            {
                return false;
            }

            var stringCategoryIds = product.categoryIds.Split(',');
            var categoryIds = new List<int>(stringCategoryIds.Length);
            
            foreach (var id in stringCategoryIds)
            {
                categoryIds.Add(int.Parse(id));
            }
            
            foreach (var line in productionLineDtos)
            {
                if (line.status != ProductionLineStatus.SCRAPPED && categoryIds.Contains(line.productionLineTemplateId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}