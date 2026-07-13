using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;
using Examples.TestDoubles;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts;

/// <summary>
/// Pattern 4: Ghost.
/// </summary>
/// <remarks>
/// <para>Generated with Google Gemini Pro 2.5.</para>
/// </remarks>
public class GhostTests
{
    [Fact]
    public void When_AccessingAnyPropertyOnGhost_Then_LoadsFullState()
    {
        // Arrange: Create a ghost object with only an ID.
        var customer = new Customer(4);

        // Assert: No DB access occurs immediately after creating a ghost object.
        Assert.Equal(0, customer.GetRepositoryCallCount());

        // Act: First access the Name property.
        var name = customer.Name;

        // Assert: The first property access loads the full data, resulting in one DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal("Ghost Customer 4", name);

        // Act: Next, access the Orders property.
        var orders = customer.Orders;

        // Assert: Since the data has already been loaded, there is no increase in DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal(2, orders.Count);
    }

    [Fact(DisplayName = "PofEAA 11.3.6 - Using Ghost Pattern")]
    public void When_UsingGhostPattern_Then_LoadsRelatedDataCorrectly()
    {
        var dbMockEmployee = DbMockFactory.CreateDbMocks(new[] {
            new { Id = 100L, Name = "ALICE", DepartmentId = 200L },
        });

        var dbMockDepartment = DbMockFactory.CreateDbMocks(new[] {
            new { Id = 200L, Name = "SALES"},
        });

        var dbMockTimeRecord = DbMockFactory.CreateDbMocks(new[] {
            new { Id = 301L, RecordedAt = DateTime.Parse("2025-10-02T09:00:00+9:00")},
            new { Id = 302L, RecordedAt = DateTime.Parse("2025-10-03T09:00:00+9:00")},
            new { Id = 303L, RecordedAt = DateTime.Parse("2025-10-06T09:00:00+9:00")},
        });

        MapperRegistry.Register<Employee>(new EmployeeMapper(dbMockEmployee.Connection));
        MapperRegistry.Register<Department>(new DepartmentMapper(dbMockDepartment.Connection));
        MapperRegistry.Register<TimeRecord>(new TimeRecordMapper(dbMockTimeRecord.Connection));
        DataSource.Init(MapperRegistry.Instance);

        Employee emp = new Employee(100);
        // First, it is initialized with Ghost.
        Assert.True(emp.IsGhost);

        // Accessing the Name property loads the data.
        var name = emp.Name;
        Assert.False(emp.IsGhost);
        Assert.Equal("ALICE", name);

        // Immediately after loading, the TimeRecords property collection is initialized with Ghost.
        var timeRecords = emp.TimeRecords;
        Assert.True(timeRecords.IsGhost);

        // Accessing the TimeRecord.Count property loads the data.
        var timeRecordCount = timeRecords.Count;
        Assert.False(timeRecords.IsGhost);
        Assert.Equal(3, timeRecordCount);

        // Individual TimeRecords are pre-loaded when the collection is loaded, avoiding ripple loading.
        var timeRecordRow0 = timeRecords[0];
        Assert.False(timeRecordRow0.IsGhost);

        // The Department property is also initially initialized to Ghost.
        var dept = emp.Department;
        Assert.True(dept is not null);
        Assert.True(dept.IsGhost);

        // Accessing the Department.Name property loads the data.
        var deptName = dept.Name;
        Assert.False(dept.IsGhost);
        Assert.Equal("SALES", deptName);

        dbMockEmployee.VerifyAll();
        dbMockDepartment.VerifyAll();
        dbMockTimeRecord.VerifyAll();
    }
}
