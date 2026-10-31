using Microsoft.EntityFrameworkCore;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
    public string Priority { get; set; } = "medium";
    public DateTime? DueDate { get; set; }
}

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> o) : base(o) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}

public static class TaskValidator
{
    static readonly string[] Allowed = { "low", "medium", "high" };

    public static List<string> Validate(TaskItem t)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(t.Title))
            errors.Add("Title is required.");
        else if (t.Title.Length > 100)
            errors.Add("Title must be 100 characters or less.");
        if (!Allowed.Contains((t.Priority ?? "").ToLower()))
            errors.Add("Priority must be low, medium or high.");
        return errors;
    }
}
