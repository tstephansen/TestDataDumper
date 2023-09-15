using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestDataDumper.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DumperContext>
{
    public DumperContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DumperContext>();
#if !MAC
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDataDumperDb;Integrated Security=True;MultipleActiveResultSets=True;");
#else
        optionsBuilder.UseSqlServer(
            "Server=localhost,14333;Database=TestDataDumperDb;Trusted_Connection=False;user id=admin;password=Password123;MultipleActiveResultSets=false;Encrypt=False;TrustServerCertificate=True;");
#endif
        return new DumperContext(optionsBuilder.Options);
    }
}