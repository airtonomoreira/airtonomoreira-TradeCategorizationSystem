using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Application.Service;
namespace TradeCategorizationSystem.Application
{
    public class TradeCategorizationService
    {
        private readonly List<ICategoryStrategy> _strategies;
        private readonly CategoryService _categoryService; 
        public TradeCategorizationService(List<ICategoryStrategy> strategies, CategoryService categoryService)
        {
            _categoryService = categoryService; 
            strategies.Add(new ExpiredCategoryStrategy());
            strategies.Add(new RiskCategoryStrategy(_categoryService));
            
            _strategies = strategies; 
        }

        public async Task<string> CategorizeTradeAsync(ITrade trade, DateTime referenceDate)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(); 
            foreach (var category in categories)
            {

                if (IsTradeExpired(trade, referenceDate))
                {
                    return "EXPIRED"; 
                }

                if (trade.Value >= category.InitialValue && trade.Value <= category.FinalValue &&
                    trade.ClientSector.ToUpper() == category.ClientSector.ToUpper())
                {
                    return category.Name; 
                }
            }
            return "Uncategorized"; 
        } 


        private bool IsTradeExpired(ITrade trade, DateTime referenceDate)
        {
            return trade.NextPaymentDate < referenceDate.AddDays(-30); 
        }

    }
}
