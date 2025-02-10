using System;
using System.Collections.Generic;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Application;

namespace TradeCategorizationSystem.Application.Service
{
    public class RiskCategoryStrategy : ICategoryStrategy
    {
        private readonly CategoryService _categoryService;

        public RiskCategoryStrategy(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public string Categorize(ITrade trade) 
        {
            var categories = _categoryService.GetAllCategoriesAsync().Result; 
            foreach (var category in categories)
            {
                if (trade.Value >= category.InitialValue && trade.Value <= category.FinalValue &&
                    trade.ClientSector.ToUpper() == category.ClientSector.ToUpper())
                {
                    return category.Name; 
                }
            }
            return "UNCATEGORIZED";
        }
    }
}

