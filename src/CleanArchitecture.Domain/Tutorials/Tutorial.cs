using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Tutorials;

public class Tutorial : Entity
{
    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public bool Published { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Tutorial(
        string title,
        string description,
        bool published,
        DateTime createdAt,
        Guid? id = null)
            : base(id ?? Guid.NewGuid())
    {
        Title = title;
        Description = description;
        Published = published;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public void Update(string title, string description, bool published, DateTime updatedAt)
    {
        Title = title;
        Description = description;
        Published = published;
        UpdatedAt = updatedAt;
    }

    private Tutorial()
    {
    }
}
