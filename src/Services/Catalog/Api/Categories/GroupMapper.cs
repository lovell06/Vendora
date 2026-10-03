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