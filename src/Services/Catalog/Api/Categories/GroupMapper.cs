using Vendora.Services.Catalog.Api.Categories.Create;
using Vendora.Services.Catalog.Api.Categories.Delete;
using Vendora.Services.Catalog.Api.Categories.Get;
using Vendora.Services.Catalog.Api.Categories.List;
using Vendora.Services.Catalog.Api.Categories.Restore;
using Vendora.Services.Catalog.Api.Categories.Update;

namespace Vendora.Services.Catalog.Api.Categories;

public static class GroupMapper
{
    public static void MapCategories(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/categories").WithTags("Categories");

        group.MapCreateCategoryEndpoint();
        group.MapListCategoriesEndpoint();
        group.MapGetCategoryEndpoint();
        group.MapUpdateCategoryEndpoint();
        group.MapDeleteCategoryEndpoint();
        group.MapRestoreCategoryEndpoint();
    }
}