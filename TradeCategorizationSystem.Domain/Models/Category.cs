using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Domain
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double InitialValue { get; private set; }
        public double FinalValue { get; private set; }
        public string ClientSector { get; private set; }
        public bool IsActive { get; private set; }

        public Category(string name, double initialValue, double? finalValue, string clientSector, bool isActive)
        {
            Name = name;
            InitialValue = initialValue;
            FinalValue = finalValue ?? double.MaxValue;
            ClientSector = clientSector;
            IsActive = isActive;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
