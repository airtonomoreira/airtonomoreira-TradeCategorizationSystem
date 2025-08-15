using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Domain
{
    public class Trade
    {
        public double Value { get; private set; }
        public string ClientSector { get; private set; }
        public DateTime NextPaymentDate { get; private set; }

        private Trade() 
        { 
            ClientSector = string.Empty;
        }

        public Trade(double value, string clientSector, DateTime nextPaymentDate)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Trade value must be positive.", nameof(value));
            }

            if (string.IsNullOrWhiteSpace(clientSector))
            {
                throw new ArgumentException("Client sector must be specified.", nameof(clientSector));
            }

            Value = value;
            ClientSector = clientSector;
            NextPaymentDate = nextPaymentDate;
        }
        public override string ToString()
        {
            return $"Trade: Value={Value}, ClientSector={ClientSector}, NextPaymentDate={NextPaymentDate}";
        }
    }

}