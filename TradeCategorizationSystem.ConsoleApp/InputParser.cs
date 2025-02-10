using System;

public class InputParser
{
    public (double value, string clientSector, DateTime nextPaymentDate) ParseTradeInput(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid input format. Expected format: <value> <clientSector> <nextPaymentDate>");
        }

        double value = double.Parse(parts[0]);
        string clientSector = parts[1];
        DateTime nextPaymentDate = DateTime.Parse(parts[2]);

        return (value, clientSector, nextPaymentDate);
    }
}
