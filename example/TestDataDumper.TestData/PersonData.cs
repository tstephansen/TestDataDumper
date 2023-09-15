using TestDataDumper.Data.Entities;

namespace TestDataDumper.TestData;

public static partial class PersonData
{
    public static List<Person> LoadPeople() => new List<Person>
    {
          new Person
          {
            Id = new Guid("74c150a1-29a8-44d7-a12b-21d41b5367ec"),
            FirstName = "Rebecca",
            LastName = "Lane",
            AddressId = new Guid("59f1f53b-5af5-434e-9215-581cbe98b106")
          },
          new Person
          {
            Id = new Guid("4eb9c29b-d843-46d8-826e-a5821f8ac4f6"),
            FirstName = "James",
            LastName = "Maxwell",
            AddressId = new Guid("887257ed-a55c-4837-bf23-ac2c74f6c65a")
          },
          new Person
          {
            Id = new Guid("fa79a427-a880-4823-b6ab-be06184c2010"),
            FirstName = "John",
            LastName = "Smith",
            AddressId = new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17")
          },
          new Person
          {
            Id = new Guid("412dfae8-e2e0-47f3-9762-c32004c97115"),
            FirstName = "Jane",
            LastName = "Smith",
            AddressId = new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17")
          }
    };
}
