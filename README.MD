# Test Data Dumper
## Description
This library takes your exported data and creates classes with a "Load" method that can be used in your unit test projects.

## The Goal
The goal of this project was to create a library that exports test data from a production db which will allow me to run my tests against the actual provider instead of the InMemory provider.

## The Process
It took me a while to come up with a good process for creating unit test data. After many years I found a solution that works great for me. 
Below is an outline of what I do when creating unit test. This assumes you are exporting data from a production SQL Server database.
This also assumes that you aren't using migrations to manage schema changes in production (you can get around this by creating a separate project for your migrations and using that for your unit tests instead).

1. Create a unit test that exports the data and run that "test" (you could also do this in a console app but since we're running tests anyway it just makes sense to me to create a test class instead).
   - Add the EF Core and ObjectDumper.net NuGet packages to the project. 
   - Create the [TestDataGenerator](example/TestDataDumper.Tests/Core/TestDataGenerator.cs) class.
   - Run the test that exports the data.
   - Comment out the TestClass and TestMethod attributes in that class so it doesn't run when you're running your unit tests.
2. Add the following method to your DbContext class (this is using the entities in the example but you would use your entities instead) and call the method at the end of the OnModelCreating(ModelBuilder modelBuilder) method.
   ```csharp
   private void AddSeedData(ModelBuilder modelBuilder)
   {
       modelBuilder.Entity<Address>().HasData(AddressData.LoadAddresses());
       modelBuilder.Entity<Person>().HasData(PersonData.LoadPeople());
       modelBuilder.Entity<Pet>().HasData(PetData.LoadPets());
       modelBuilder.Entity<PetOwner>().HasData(PetOwnerData.LoadPetOwners());
   }
   ```
3. Add a new migration and comment out the call to AddSeedData in the OnModelCreating method.
   - `dotnet ef migrations add SeedData` 
4. Update the database.
   - `dotnet ef database update`
5. Create a backup of the test database you just created and add that file to the project (set to Content and PreserveNewest).
6. Create a TestFixture, TestDbContextManager, and LocalDbManager classes (you can use the ones in the example as a reference).
7. Customize and add the following methods to your unit test classes to restore the database and use a transaction to rollback the changes between the tests.
   ```csharp
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
      var context = TestDbContextManager.CreateDumperContext(TestDbName);
      context.Database.EnsureDeleted();
   }
   
   [TestCleanup]
   public void TestCleanup()
   {
       DumperTransaction.Rollback();
   }
   ```
8. Add a constructor to the unit test class that calls the CreateDbContext() method in the TestFixture.
9. That's it! At this point you should have a test db that is recreated every time you run unit tests.