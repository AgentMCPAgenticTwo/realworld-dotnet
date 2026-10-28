using CleanArchitecture.Application.Categories.Queries.ListCategories;

namespace TestCommon.Categories;

public static class CategoryQueryFactory
{
    public static ListCategoriesQuery CreateListCategoriesQuery()
    {
        return new ListCategoriesQuery();
    }
}
