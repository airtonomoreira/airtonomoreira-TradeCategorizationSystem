using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Domain.Interfaces
{
    public interface ICategoryStrategy
    {
        string Categorize(Trade trade);
    }
}