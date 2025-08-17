using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Infrastructure;
using TradeCategorizationSystem.Domain;
using Microsoft.EntityFrameworkCore;
using TradeCategorizationSystem.Application.Service;

public class MenuHandler
{
    public void ShowMenu()
    {
        Console.WriteLine("1. Add Category");
        Console.WriteLine("2. Update Category");
        Console.WriteLine("3. Delete Category");
        Console.WriteLine("4. List Categories");
        Console.WriteLine("5. Categorize Trade");
        Console.WriteLine("0. Exit");
    }

    public async Task HandleUserInput()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TradeCategoryDbContext>();
        optionsBuilder.UseSqlite("Data Source=tradecategories.db");
        var dbContext = new TradeCategoryDbContext(optionsBuilder.Options);
        var unitOfWork = new UnitOfWork(dbContext);
        var categoryService = new CategoryService(unitOfWork);

        string input;
        do
        {
            Console.Write("Select an option: \n");
            ShowMenu();
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    try
                    {
                        await ListCategoriesAsync(categoryService);

                    }
                    catch (Exception)
                    {

                        Console.WriteLine("An error occurred while listing categories. Please try again.");
                    }
                    await AddCategoryAsync(categoryService);
                    break;

                case "2":
                    await UpdateCategoryAsync(categoryService);
                    break;

                case "3":
                    await DeleteCategoryAsync(categoryService);
                    break;

                case "4":
                    await ListCategoriesAsync(categoryService);
                    break;

                case "5":
                    await CategorizeTradeAsync(categoryService);
                    break;

                case "0":
                    Console.WriteLine("Exiting the application.");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (input != "0");
    }

    private async Task AddCategoryAsync(CategoryService categoryService)
    {
        var categoryDto = new CategoryDto();
        Console.Write("Enter category name: ");
        categoryDto.Name = Console.ReadLine().ToUpper();
        Console.Write("Enter initial value: ");
        double initialValue;
        while (!double.TryParse(Console.ReadLine(), out initialValue))
        {
            Console.Write("Invalid input. Please enter a valid initial value: ");
        }
        categoryDto.InitialValue = initialValue;

        Console.Write("Enter final value (or leave blank for MaxValue): ");
        var finalValueInput = Console.ReadLine();
        categoryDto.FinalValue = string.IsNullOrEmpty(finalValueInput) ? double.MaxValue : double.Parse(finalValueInput);

        Console.Write("Enter client sector: ");
        categoryDto.ClientSector = Console.ReadLine();

        Console.Write("Is the category active? (0 for false, 1 for true): ");
        string isActiveInput = Console.ReadLine();
        categoryDto.IsActive = isActiveInput == "1";

        await categoryService.AddCategoryAsync(categoryDto);
        Console.WriteLine("Category added successfully.");
    }

    private async Task UpdateCategoryAsync(CategoryService categoryService)
    {
        Console.Write("Enter the name of the category to update: ");
        var categoryNameToUpdate = Console.ReadLine().ToUpper().Trim(); // Normalize input
        if (string.IsNullOrWhiteSpace(categoryNameToUpdate))
        {
            Console.WriteLine("Category name cannot be empty.");
            return;
        }

        var existingCategory = await categoryService.GetCategoryByIdAsync(categoryNameToUpdate);
        if (existingCategory == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        var categoryDtoToUpdate = new CategoryDto
        {
            Name = existingCategory.Name,
            InitialValue = existingCategory.InitialValue,
            FinalValue = existingCategory.FinalValue,
            ClientSector = existingCategory.ClientSector,
            IsActive = existingCategory.IsActive
        };

        Console.Write("Enter new category name (leave blank to keep current value): ");
        var newCategoryName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newCategoryName) && newCategoryName != existingCategory.Name)
        {
   
            var existingWithNewName = await categoryService.GetCategoryByIdAsync(newCategoryName.ToUpper());
            if (existingWithNewName != null)
            {
                Console.WriteLine("A category with this name already exists. Please choose a different name.");
                return;
            }
            categoryDtoToUpdate.Name = newCategoryName.ToUpper();
        }

        Console.Write("Enter new initial value (leave blank to keep current value): ");
        var newInitialValueInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newInitialValueInput))
        {
            double newInitialValue = double.Parse(newInitialValueInput);
            categoryDtoToUpdate.InitialValue = newInitialValue;
        }

        Console.Write("Enter new final value (or leave blank for MaxValue): ");
        var newFinalValueInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newFinalValueInput))
        {
            double newFinalValue = double.Parse(newFinalValueInput);
            categoryDtoToUpdate.FinalValue = newFinalValue;
        }

        Console.Write("Enter new client sector (leave blank to keep current value): ");
        var newClientSector = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newClientSector))
        {
            categoryDtoToUpdate.ClientSector = newClientSector;
        }

        Console.Write("Is the category active? (0 for false, 1 for true, leave blank to keep current value): ");
        var isActiveInputUpdate = Console.ReadLine();
        if (isActiveInputUpdate.Trim() == "1")
        {
            categoryDtoToUpdate.IsActive = true;
        }
        else if (isActiveInputUpdate.Trim() == "0")
        {
            categoryDtoToUpdate.IsActive = false;
        }

        if (await categoryService.CheckForOverlappingRangesAsync(categoryDtoToUpdate))
        {
            Console.WriteLine("The new values overlap with an existing range. Update not allowed.");
            return;
        }

        await categoryService.UpdateCategoryAsync(categoryDtoToUpdate);
        Console.WriteLine("Category updated successfully.");
    }

    private async Task DeleteCategoryAsync(CategoryService categoryService)
    {
        Console.Write("Enter the name of the category to delete: ");
        var categoryNameToDelete = Console.ReadLine();
        await categoryService.DeleteCategoryAsync(categoryNameToDelete);
        Console.WriteLine("Category deleted successfully.");
    }

    private async Task ListCategoriesAsync(CategoryService categoryService)
    {
        var existingCategories = await categoryService.GetAllCategoriesAsync();
        foreach (var category in existingCategories)
        {
            Console.WriteLine($"Name: {category.Name}, Initial Value: {category.InitialValue}, Final Value: {category.FinalValue}, Client Sector: {category.ClientSector}, Active: {category.IsActive}");
        }
    }

    private async Task CategorizeTradeAsync(CategoryService categoryService)
    {
        Console.Write("Enter reference date (MM/dd/yyyy): ");
        var referenceDateInput = Console.ReadLine();
        DateTime referenceDate;
        if (!DateTime.TryParse(referenceDateInput, out referenceDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        Console.Write("Enter the number of trades: ");
        if (!int.TryParse(Console.ReadLine(), out int numberOfTrades) || numberOfTrades <= 0)
        {
            Console.WriteLine("Invalid number of trades.");
            return;
        }


        var strategies = new List<ICategoryStrategy>
        {
            new ExpiredCategoryStrategy(),
            new RiskCategoryStrategy(categoryService)
        };

        var tradeCategorizationService = new TradeCategorizationService(strategies, categoryService);
        List<string> tradeCategories = new List<string>();

        for (int i = 0; i < numberOfTrades; i++)
        {
            Console.Write($"Enter trade {i + 1} (value clientSector nextPaymentDate): ");
            var tradeInput = Console.ReadLine().Split(' ');
            if (tradeInput.Length != 3 ||
                !double.TryParse(tradeInput[0], out double tradeValue) ||
                string.IsNullOrWhiteSpace(tradeInput[1]) ||
                !DateTime.TryParse(tradeInput[2], out DateTime nextPaymentDate))
            {
                Console.WriteLine("Invalid input format. Please enter in the format: value clientSector nextPaymentDate");
                i--;
                continue;
            }


            ITrade trade = new Trade(tradeValue, tradeInput[1], nextPaymentDate);

            string category = await tradeCategorizationService.CategorizeTradeAsync(trade, referenceDate);

            tradeCategories.Add(category);
        }

        foreach (var category in tradeCategories)
        {
            Console.WriteLine(category);
        }
    }
}

