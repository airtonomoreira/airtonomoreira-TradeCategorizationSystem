using System;
using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Application.Service
{
    public class ExpiredCategoryStrategy : ICategoryStrategy
    {
        public string Categorize(ITrade trade)
        {
            if (trade.NextPaymentDate < DateTime.Now.AddDays(-30))
            {
                return "EXPIRED";
            }
            return "";
        }
    }
}
