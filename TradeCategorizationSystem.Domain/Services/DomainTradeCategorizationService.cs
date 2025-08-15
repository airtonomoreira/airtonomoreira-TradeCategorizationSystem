using System.Collections.Generic;
using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Domain.Services
{
    public class DomainTradeCategorizationService
    {
        private readonly IEnumerable<Category> _categories;

        public DomainTradeCategorizationService(IEnumerable<Category> categories)
        {
            _categories = categories;
        }

        public string Categorize(Trade trade)
        {
            // Your categorization logic here, using the _categories
            // For example:
            foreach (var category in _categories)
            {
                if (trade.Value >= category.InitialValue && trade.Value <= category.FinalValue && trade.ClientSector == category.ClientSector)
                {
                    return category.Name;
                }
            }

            return "Uncategorized";
        }
    }
}