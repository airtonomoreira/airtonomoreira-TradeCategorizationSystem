using TradeCategorizationSystem.Application;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Application.Service;

public class MenuHandler
{
    private readonly CategoryService _categoryService;
    private readonly TradeCategorizationService _tradeCategorizationService;
    private readonly InputParser _inputParser;

    public MenuHandler(
        CategoryService categoryService,
        TradeCategorizationService tradeCategorizationService,
        InputParser inputParser)
    {
        _categoryService = categoryService;
        _tradeCategorizationService = tradeCategorizationService;
        _inputParser = inputParser;
    }

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
        string input;
        do
        {
            Console.Write("Select an option: \n");
            ShowMenu();
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await AddCategoryAsync();
                    break;

                case "2":
                    await UpdateCategoryAsync();
                    break;

                case "3":
                    await DeleteCategoryAsync();
                    break;

                case "4":
                    await ListCategoriesAsync();
                    break;

                case "5":
                    await CategorizeTradeAsync();
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

    private async Task AddCategoryAsync()
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

        await _categoryService.AddCategoryAsync(categoryDto);
        Console.WriteLine("Category added successfully.");
    }

    private async Task UpdateCategoryAsync()
    {
        Console.Write("Enter the name of the category to update: ");
        var categoryNameToUpdate = Console.ReadLine().ToUpper().Trim(); // Normalize input
        if (string.IsNullOrWhiteSpace(categoryNameToUpdate))
        {
            Console.WriteLine("Category name cannot be empty.");
            return;
        }

        var existingCategory = await _categoryService.GetCategoryByIdAsync(categoryNameToUpdate);
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
   
            var existingWithNewName = await _categoryService.GetCategoryByIdAsync(newCategoryName.ToUpper());
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

        if (await _categoryService.CheckForOverlappingRangesAsync(categoryDtoToUpdate))
        {
            Console.WriteLine("The new values overlap with an existing range. Update not allowed.");
            return;
        }

        await _categoryService.UpdateCategoryAsync(categoryDtoToUpdate);
        Console.WriteLine("Category updated successfully.");
    }

    private async Task DeleteCategoryAsync()
    {
        Console.Write("Enter the name of the category to delete: ");
        var categoryNameToDelete = Console.ReadLine();
        await _categoryService.DeleteCategoryAsync(categoryNameToDelete);
        Console.WriteLine("Category deleted successfully.");
    }

    private async Task ListCategoriesAsync()
    {
        var existingCategories = await _categoryService.GetAllCategoriesAsync();
        foreach (var category in existingCategories)
        {
            Console.WriteLine($"Name: {category.Name}, Initial Value: {category.InitialValue}, Final Value: {category.FinalValue}, Client Sector: {category.ClientSector}, Active: {category.IsActive}");
        }
    }

    private async Task CategorizeTradeAsync()
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

        List<string> tradeCategories = new List<string>();

        for (int i = 0; i < numberOfTrades; i++)
        {
            Console.Write($"Enter trade {i + 1} (value clientSector nextPaymentDate): ");
            var tradeInput = Console.ReadLine();

            try
            {
                var (tradeValue, clientSector, nextPaymentDate) = _inputParser.ParseTradeInput(tradeInput);
                ITrade trade = new Trade(tradeValue, clientSector, nextPaymentDate);
                string category = await _tradeCategorizationService.CategorizeTradeAsync(trade, referenceDate);
                tradeCategories.Add(category);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                i--; // Tenta novamente a mesma entrada de trade
                continue;
            }
        }

        foreach (var category in tradeCategories)
        {
            Console.WriteLine(category);
        }
    }
}
