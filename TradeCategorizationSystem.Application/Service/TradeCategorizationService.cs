using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Domain.Interfaces;

namespace TradeCategorizationSystem.Application
{
    public class TradeCategorizationService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ICategoryStrategy _categoryStrategy;

        public TradeCategorizationService(ITradeRepository tradeRepository, ICategoryStrategy categoryStrategy)
        {
            _tradeRepository = tradeRepository;
            _categoryStrategy = categoryStrategy;
        }

        public IEnumerable<string> CategorizeTrades()
        {
            var trades = _tradeRepository.GetAll();
            var categorizedTrades = new List<string>();

            foreach (var trade in trades)
            {
                categorizedTrades.Add(_categoryStrategy.Categorize(trade));
            }

            return categorizedTrades;
        }
    }
}
