using Microsoft.EntityFrameworkCore;
using TestDataDumper.Data;

namespace TestDataDumper.Tests.Core;

/// <summary>
/// This class is responsible for creating the DbContext during tests.
/// </summary>
public static class TestDbContextManager
{
    private static bool _isCreated;
    
    /// <summary>
    ///     Creates an instance of the DumperContext DbContext class that points to the test database.
    /// </summary>
    /// <param name="dbName">The database name.</param>
    /// <returns>A DumperContext.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Throws an ArgumentNullException if the dbName parameter is null.
    /// </exception>
    public static DumperContext CreateDbContext(string dbName)
    {
        if (!dbName.Contains("TestDataDumperDb_"))
            throw new ArgumentNullException(nameof(dbName), "Database Name Not Set");
        if (!_isCreated)
            RestoreDatabase(dbName);
#if !MAC
        var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;MultipleActiveResultSets=True;";
#else
        var connectionString =
            $"Server=localhost,14333;Database={dbName};Trusted_Connection=False;user id=admin;password=Password123;MultipleActiveResultSets=false;Encrypt=False;TrustServerCertificate=True;";
#endif
        var builder = new DbContextOptionsBuilder<DumperContext>().UseSqlServer(connectionString);
        return new DumperContext(builder.Options);
    }

    /// <summary>
    /// Restores the database.
    /// </summary>
    /// <param name="dbName">The name of the database.</param>
    /// <exception cref="Exception">Throws an exception if the database is not restored.</exception>
    public static void RestoreDatabase(string dbName)
    {
        _isCreated = LocalDbManager.RestoreDatabase(dbName, GetTestDbFolderPath(), "TestDataDumperDb.bak");
        if (!_isCreated)
            throw new Exception("SQL Database Not Created");
    }

    private static string GetTestDbFolderPath()
    {
#if !MAC
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDbs");
#else
        return "/var/opt/mssql/data";
#endif
    }
}