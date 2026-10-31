using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDb>(o => o.UseSqlite("Data Source=tasks.db"));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDb>().Database.EnsureCreated();

app.MapGet("/tasks", async (AppDb db) => await db.Tasks.ToListAsync());

app.MapGet("/tasks/{id}", async (int id, AppDb db) =>
    await db.Tasks.FindAsync(id) is TaskItem t ? Results.Ok(t) : Results.NotFound());

app.MapPost("/tasks", async (TaskItem t, AppDb db) =>
{
    db.Tasks.Add(t);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{t.Id}", t);
});

app.MapPut("/tasks/{id}", async (int id, TaskItem input, AppDb db) =>
{
    var t = await db.Tasks.FindAsync(id);
    if (t is null) return Results.NotFound();
    t.Title = input.Title;
    t.IsDone = input.IsDone;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (int id, AppDb db) =>
{
    var t = await db.Tasks.FindAsync(id);
    if (t is null) return Results.NotFound();
    db.Tasks.Remove(t);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
}

class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> o) : base(o) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}
