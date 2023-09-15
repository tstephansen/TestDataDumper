using TestDataDumper.Data.Entities;

namespace TestDataDumper.TestData;

public static partial class PetData
{
    public static List<Pet> LoadPets() => new List<Pet>
    {
          new Pet
          {
            Id = new Guid("0ede9947-28af-489f-b15d-658a4df73c8f"),
            Name = "Skipper"
          },
          new Pet
          {
            Id = new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7"),
            Name = "Munson"
          }
    };
}
