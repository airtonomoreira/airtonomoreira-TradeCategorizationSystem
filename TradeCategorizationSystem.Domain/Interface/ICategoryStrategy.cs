using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Domain
{
    public interface ICategoryStrategy
    {
        string Categorize(ITrade trade);
    }

}