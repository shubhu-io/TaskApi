using Xunit;

public class TaskValidatorTests
{
    [Fact]
    public void ValidTask_HasNoErrors()
    {
        var t = new TaskItem { Title = "Learn .NET", Priority = "high" };
        Assert.Empty(TaskValidator.Validate(t));
    }

    [Fact]
    public void EmptyTitle_ReturnsError()
    {
        var t = new TaskItem { Title = "  ", Priority = "low" };
        Assert.Contains("Title is required.", TaskValidator.Validate(t));
    }

    [Fact]
    public void TooLongTitle_ReturnsError()
    {
        var t = new TaskItem { Title = new string('a', 101), Priority = "low" };
        Assert.Contains("Title must be 100 characters or less.", TaskValidator.Validate(t));
    }

    [Fact]
    public void InvalidPriority_ReturnsError()
    {
        var t = new TaskItem { Title = "Test", Priority = "urgent" };
        Assert.Contains("Priority must be low, medium or high.", TaskValidator.Validate(t));
    }

    [Theory]
    [InlineData("low")]
    [InlineData("medium")]
    [InlineData("HIGH")]
    public void AllowedPriorities_AreAccepted(string p)
    {
        var t = new TaskItem { Title = "Test", Priority = p };
        Assert.Empty(TaskValidator.Validate(t));
    }
}
