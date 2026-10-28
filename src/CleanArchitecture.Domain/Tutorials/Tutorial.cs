using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Tutorials;

public class Tutorial : Entity
{
    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public bool Published { get; private set; }

    public Guid? CategoryId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Tutorial(
        string title,
        string description,
        bool published,
        DateTime createdAt,
        Guid? categoryId = null,
        Guid? id = null)
            : base(id ?? Guid.NewGuid())
    {
        Title = title;
        Description = description;
        Published = published;
        CategoryId = categoryId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public void Update(string title, string description, bool published, Guid? categoryId, DateTime updatedAt)
    {
        Title = title;
        Description = description;
        Published = published;
        CategoryId = categoryId;
        UpdatedAt = updatedAt;
    }

    private Tutorial()
    {
    }
}
