using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TestDataDumper.Data;

namespace TestDataDumper.Tests.Core;

/// <summary>
///     This class is used to export data from your database to create test data.
/// </summary>
//[TestClass]
public class TestDataGenerator
{
    /// <summary>
    ///     When you run this "test" it will export the entities in the database as classes
    ///     to the TestDataDumper.TestData project.
    /// </summary>
    //[TestMethod]
    public async Task CreateTestDataAsync()
    { 
        // Get the base directory of this project.
        var baseDir = AppDomain.CurrentDomain.BaseDirectory[..^17];
        var solutionDirectory = new DirectoryInfo(baseDir).Parent;
        Debug.Assert(solutionDirectory != null, nameof(solutionDirectory) + " != null");
        // Get the location of the folder you want the exported data to be written to.
        var folderPath = Path.Combine(solutionDirectory.FullName, "TestDataDumper.TestData");
        // Create the folder if it doesn't exist.
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        // Create the DbContext (this should point to the db you want to export data from)
        var context = new DumperContext(new DbContextOptionsBuilder<DumperContext>()
            .UseSqlServer(GetConnectionString()).Options);
        // Get the data you want to export from the db
        var addresses = await context.Addresses.ToListAsync();
        var people = await context.People.ToListAsync();
        var pets = await context.Pets.ToListAsync();
        var petOwners = await context.PetOwners.ToListAsync();
        // Set the options you want to use for the dump.
        var options = new TestDataDumperOptions(folderPath, "TestDataDumper.TestData",
            additionalNamespaces: new[] { "TestDataDumper.Data.Entities" });
        // Create the dump for each entity
        if (addresses.Count > 0)
        {
            var addressDump = addresses.Dump(new DumpOptions
            {
                DumpStyle = DumpStyle.CSharp,
                ExcludeProperties = new[] { "People" }
            });
            TestDataDumper.CreateClass("Address", addressDump, options, "Addresses");
        }
        if (people.Count > 0)
        {
            var peopleDump = people.Dump(new DumpOptions
            {
                DumpStyle = DumpStyle.CSharp,
                ExcludeProperties = new[] { "Address", "PetOwners" }
            });
            TestDataDumper.CreateClass("Person", peopleDump, options, "People");
        }
        if (pets.Count > 0)
        {
            var petsDump = pets.Dump(new DumpOptions
            {
                DumpStyle = DumpStyle.CSharp,
                ExcludeProperties = new[] { "PetOwners" }
            });
            TestDataDumper.CreateClass("Pet", petsDump, options);
        }
        if (petOwners.Count > 0)
        {
            var ownersDump = petOwners.Dump(new DumpOptions
            {
                DumpStyle = DumpStyle.CSharp,
                ExcludeProperties = new[] { "Owner", "Pet" }
            });
            TestDataDumper.CreateClass("PetOwner", ownersDump, options);
        }
    }
    
    private static string GetConnectionString()
    {
#if MAC
        return $"Server=localhost,14333;Database=TestDataDumperDb;Trusted_Connection=False;user id=admin;password=Password123;MultipleActiveResultSets=false;Encrypt=False;TrustServerCertificate=True;";
#else
        return $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog={TestDbName};Integrated Security=True;MultipleActiveResultSets=True;";
#endif
    }
}