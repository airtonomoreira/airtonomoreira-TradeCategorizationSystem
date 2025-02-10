using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Application
{
    public class CategoryDto
    {
        public string? Name { get; set; }
        public double InitialValue { get; set; }
        public double FinalValue { get; set; }
        public string? ClientSector { get; set; }
        public bool IsActive { get; set; }
    }

}
