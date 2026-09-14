namespace Vendora.Services.Catalog.Domain.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<Category>> GetAsync(CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
    void Add(Category category);
}