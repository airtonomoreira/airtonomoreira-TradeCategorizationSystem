﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Application.Service;
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
            // Registra as estratégias para que possam ser injetadas no TradeCategorizationService
            .AddScoped<ICategoryStrategy, ExpiredCategoryStrategy>()
            .AddScoped<ICategoryStrategy, RiskCategoryStrategy>()
            .AddScoped<TradeCategorizationService>()
            .AddScoped<InputParser>()
            .AddScoped<MenuHandler>()
            .BuildServiceProvider();

        Console.WriteLine("Trade Categorization System!");

        // Cria um escopo para resolver e usar os serviços
        using (var scope = serviceProvider.CreateScope())
        {
            var menuHandler = scope.ServiceProvider.GetRequiredService<MenuHandler>();
            await menuHandler.HandleUserInput();
        }
    }
}
