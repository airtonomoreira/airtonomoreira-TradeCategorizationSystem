using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Infrastructure;
using TradeCategorizationSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace TradeCategorizationSystem.ConsoleApp
{
    public class MenuHandler
    {
        private readonly TradeCategorizationService _tradeCategorizationService;

        public MenuHandler(TradeCategorizationService tradeCategorizationService)
        {
            _tradeCategorizationService = tradeCategorizationService;
        }

        public async Task ShowMenuAsync()
        {
           Console.WriteLine("Welcome to the Trade Categorization System!");
           Console.WriteLine("1. Categorize Trade");
           Console.WriteLine("2. Exit");
           Console.Write("Select an option: ");

           var option = Console.ReadLine();

           switch (option)
           {
               case "1":
                   await CategorizeTradeAsync();
                   break;
               case "2":
                   Environment.Exit(0);
                   break;
               default:
                   Console.WriteLine("Invalid option. Please try again.");
                   await ShowMenuAsync();
                   break;
           }
        }
    }
}

