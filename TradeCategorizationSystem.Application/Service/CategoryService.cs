using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Infrastructure;

namespace TradeCategorizationSystem.Application
{
    public class CategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            var categoryDtos = new List<CategoryDto>();

  
            foreach (var category in categories)
            {
                categoryDtos.Add(new CategoryDto
                {
                    Name = category.Name,
                    InitialValue = category.InitialValue,
                    FinalValue = category.FinalValue,
                    ClientSector = category.ClientSector,
                    IsActive = category.IsActive
                });
            }

            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Name = category.Name,
                InitialValue = category.InitialValue,
                FinalValue = category.FinalValue,
                ClientSector = category.ClientSector,
                IsActive = category.IsActive
            };
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category(categoryDto.Name, categoryDto.InitialValue, categoryDto.FinalValue, categoryDto.ClientSector, categoryDto.IsActive);
            await _unitOfWork.CategoryRepository.AddCategoryAsync(category);
            Console.WriteLine("Category added successfully.");
        }

        public async Task<bool> CheckForOverlappingRangesAsync(CategoryDto categoryDto)
        {
            var category = new Category(categoryDto.Name, categoryDto.InitialValue, categoryDto.FinalValue, categoryDto.ClientSector, categoryDto.IsActive);
            return await _unitOfWork.CategoryRepository.CheckForOverlappingRangesAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category(categoryDto.Name, categoryDto.InitialValue, categoryDto.FinalValue, categoryDto.ClientSector, categoryDto.IsActive);
            await _unitOfWork.CategoryRepository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(string name)
        {
            await _unitOfWork.CategoryRepository.DeleteCategoryAsync(name);
        }
    }
}
