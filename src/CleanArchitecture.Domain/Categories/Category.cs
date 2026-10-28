using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Categories;

public class Category : Entity
{
    public string Name { get; private set; } = null!;

    public Category(string name, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        Name = name;
    }

    private Category()
    {
    }
}
