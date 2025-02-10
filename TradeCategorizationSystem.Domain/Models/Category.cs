using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Domain
{
    public class Category
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public double InitialValue { get; set; }
        public double FinalValue { get; set; } 
        public string ClientSector { get; set; }
        public bool IsActive { get; set; } = false;

   
        public Category() { }

        public Category(string name, double initialValue, double? finalValue, string clientSector, bool isActive)
        {
            Name = name;
            InitialValue = initialValue;
            FinalValue = finalValue ?? double.MaxValue; 
            ClientSector = clientSector;
            IsActive = isActive;
        }
    }
}
