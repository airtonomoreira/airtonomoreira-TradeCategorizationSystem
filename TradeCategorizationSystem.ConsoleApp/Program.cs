using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Infrastructure;
using TradeCategorizationSystem.ConsoleApp;
using System.Data.SQLite;
using TradeCategorizationSystem.Domain.Interfaces;
using TradeCategorizationSystem.Infrastructure.Data;
using TradeCategorizationSystem.Application.Service;

public class Program
{
    static async Task Main(string[] args)
    {

        var serviceProvider = new ServiceCollection()
            .AddDbContext<TradeCategoryDbContext>(options =>
                options.UseSqlite("Data Source=tradecategories.db"))
            .AddScoped<UnitOfWork>()
            .AddScoped<CategoryService>()
            .AddScoped<TradeCategorizationService>()
            .AddScoped<MenuHandler>()
            .AddSingleton(new SQLiteConnection("Data Source=tradecategories.db"))
            .AddScoped<ITradeRepository, TradeRepository>()
            .AddScoped<ICategoryStrategy, RiskCategoryStrategy>()
            .BuildServiceProvider();

        Console.WriteLine("Trade Categorization System!");

        var menuHandler = serviceProvider.GetRequiredService<MenuHandler>();

        await menuHandler.ShowMenuAsync();
    }
}