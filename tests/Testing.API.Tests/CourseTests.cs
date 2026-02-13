using Testing.API.DataAccess.Entities;

namespace Testing.API.Tests;

public class CourseTests
{
    [Fact]
    public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
    {
        // Nothing to arrange

        // Act 
        var course = new Course("Disaster Management 101");

        // Assert
        Assert.True(course.IsNew);
    }
}
