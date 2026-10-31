using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDb>(o => o.UseSqlite("Data Source=tasks.db"));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDb>().Database.EnsureCreated();

app.MapGet("/tasks", async (AppDb db, bool? done, string? search) =>
{
    var q = db.Tasks.AsQueryable();
    if (done.HasValue) q = q.Where(t => t.IsDone == done.Value);
    if (!string.IsNullOrWhiteSpace(search)) q = q.Where(t => EF.Functions.Like(t.Title, $"%{search}%"));
    return await q.OrderBy(t => t.Id).ToListAsync();
});

app.MapGet("/tasks/stats", async (AppDb db) =>
{
    var total = await db.Tasks.CountAsync();
    var completed = await db.Tasks.CountAsync(t => t.IsDone);
    return Results.Ok(new { total, completed, pending = total - completed });
});

app.MapGet("/tasks/{id}", async (int id, AppDb db) =>
    await db.Tasks.FindAsync(id) is TaskItem t ? Results.Ok(t) : Results.NotFound());

app.MapPost("/tasks", async (TaskItem t, AppDb db) =>
{
    var errors = TaskValidator.Validate(t);
    if (errors.Count > 0) return Results.BadRequest(new { errors });
    db.Tasks.Add(t);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{t.Id}", t);
});

app.MapPut("/tasks/{id}", async (int id, TaskItem input, AppDb db) =>
{
    var errors = TaskValidator.Validate(input);
    if (errors.Count > 0) return Results.BadRequest(new { errors });
    var t = await db.Tasks.FindAsync(id);
    if (t is null) return Results.NotFound();
    t.Title = input.Title;
    t.IsDone = input.IsDone;
    t.Priority = input.Priority;
    t.DueDate = input.DueDate;
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

