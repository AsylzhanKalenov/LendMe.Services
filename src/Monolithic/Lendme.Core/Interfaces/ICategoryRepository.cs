using Lendme.Core.Entities.Catalog;

namespace Lendme.Core.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
}