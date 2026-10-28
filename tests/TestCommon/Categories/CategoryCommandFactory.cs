using CleanArchitecture.Application.Categories.Commands.CreateCategory;

using TestCommon.TestConstants;

namespace TestCommon.Categories;

public static class CategoryCommandFactory
{
    public static CreateCategoryCommand CreateCreateCategoryCommand(string name = Constants.Category.Name)
    {
        return new CreateCategoryCommand(name);
    }
}
