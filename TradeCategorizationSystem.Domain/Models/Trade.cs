using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCategorizationSystem.Domain
{
    public class Trade : ITrade
    {
        public double Value { get; private set; }
        public string ClientSector { get; private set; }
        public DateTime NextPaymentDate { get; private set; }

        public Trade(double value, string clientSector, DateTime nextPaymentDate)
        {
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