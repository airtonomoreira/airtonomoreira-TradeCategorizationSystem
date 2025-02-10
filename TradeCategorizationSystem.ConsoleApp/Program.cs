﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Infrastructure;

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
            .BuildServiceProvider();

        Console.WriteLine("Trade Categorization System!");

        var menuHandler = new MenuHandler();

        await menuHandler.HandleUserInput();
    }
}
