# Test Data Dumper
## Description
This library takes your exported data and creates classes with a "Load" method that can be used in your unit test projects.

## Usage
To use this library you will need to install the ObjectDumper.net NuGet package into your project. Then load the data you want to use and call the "Dump" method.

Example usage with EF Core (this assumes the class you're exporting is called People and has an address and a collection of pets):
```csharp
public void ExportDataFromDb()
{
    // Setup
    var exportPath = "C:/Users/myself/Git/MyProject/MyProject.Tests/TestData";
    var ns = "MyProject.Tests.TestData";
    var className = "Person";
    var context = new MyDbContext();
    var people = context.People.ToList();
    var data = people.Dump(new DumpOptions
    {
        DumpStyle = DumpStyle.CSharp,
        ExcludeProperties = new[]
        {
            "Pets",
            "Address"
        }
    });
    var dataDumper = new TestDataDumper();
    var result = dataDumper.CreateClass(exportPath, ns, className, data);
    if (!result)
    {
        throw new Exception("Failed to create class");
    }
}
```