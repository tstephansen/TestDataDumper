using Microsoft.EntityFrameworkCore;
using TestDataDumper.Tests.Core;

namespace TestDataDumper.Tests;

[TestClass]
public class ExampleTests : TestFixture
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDbs");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (string.IsNullOrEmpty(TestDbName))
            TestDbName = $"TestDataDumperDb_{Guid.NewGuid()}";
        TestDbContextManager.RestoreDatabase(TestDbName);
    }
    
    [ClassCleanup]
    public static void ClassCleanUp()
    {
        var context = TestDbContextManager.CreateDbContext(TestDbName);
        context.Database.EnsureDeleted();
    }
    
    [TestCleanup]
    public void TestCleanup()
    {
        DumperTransaction.Rollback();
    }
    
    public ExampleTests()
    {
        CreateDbContext();
    }
    
    [TestMethod]
    public async Task EntityCount_ShouldBeCorrect()
    {
        // Arrange
        const int totalPeople = 4;
        const int totalPets = 2;
        const int totalAddresses = 3;
        const int totalPetOwners = 3;
        
        // Act
        var people = await DumperContext.People.CountAsync();
        var pets = await DumperContext.Pets.CountAsync();
        var addresses = await DumperContext.Addresses.CountAsync();
        var petOwners = await DumperContext.PetOwners.CountAsync();
        
        // Assert
        Assert.AreEqual(totalPeople, people);
        Assert.AreEqual(totalPets, pets);
        Assert.AreEqual(totalAddresses, addresses);
        Assert.AreEqual(totalPetOwners, petOwners);
    }

    [TestMethod]
    public async Task RenamingPeople_ShouldWork()
    {
        // Arrange
        var personId = new Guid("412DFAE8-E2E0-47F3-9762-C32004C97115");
        const string firstName = "Jackson";
        const string lastName = "Jones";
        
        // Act
        var person = await DumperContext.People.FindAsync(personId);
        person.FirstName = firstName;
        person.LastName = lastName;
        await DumperContext.SaveChangesAsync();
        
        // Assert
        var modifiedPerson = await DumperContext.People.FindAsync(personId);
        Assert.AreEqual(firstName, modifiedPerson.FirstName);
        Assert.AreEqual(lastName, modifiedPerson.LastName);
    }
}