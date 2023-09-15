using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestDataDumper.Data;

namespace TestDataDumper.Tests.Core;

/// <summary>
///     This abstract class is inherited by the test classes and has methods
///     that are used for setting up the tests.
/// </summary>
public abstract class TestFixture
{
    protected TestFixture()
    {
        if (string.IsNullOrEmpty(TestDbName))
            TestDbName = $"TestDataDumperDb_{Guid.NewGuid()}";
        SetConnectionString();
    }

    private static void SetConnectionString()
    {
#if !MAC
        _connectionString = $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog={TestDbName};Integrated Security=True;MultipleActiveResultSets=True;";
#else
        _connectionString =
            $"Server=localhost,14333;Database={TestDbName};Trusted_Connection=False;user id=admin;password=Password123;MultipleActiveResultSets=false;Encrypt=False;TrustServerCertificate=True;";
#endif
    }

    protected void CreateDbContext()
    {
        DumperContext = TestDbContextManager.CreateDbContext(TestDbName);
        DumperTransaction = DumperContext.Database.BeginTransaction();
    }

    protected static string TestDbName { get; set; } = null!;
    private static string _connectionString = null!;
    protected DumperContext DumperContext { get; set; } = null!;
    protected IDbContextTransaction DumperTransaction { get; set; } = null!;
}