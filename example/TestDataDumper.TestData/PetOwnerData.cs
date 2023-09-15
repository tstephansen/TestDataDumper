using TestDataDumper.Data.Entities;

namespace TestDataDumper.TestData;

public static partial class PetOwnerData
{
    public static List<PetOwner> LoadPetOwners() => new List<PetOwner>
    {
          new PetOwner
          {
            Id = new Guid("3e932ab2-36d3-42df-8bbb-48363142aa59"),
            OwnerId = new Guid("fa79a427-a880-4823-b6ab-be06184c2010"),
            PetId = new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7")
          },
          new PetOwner
          {
            Id = new Guid("3d968eb0-bcbe-4cbd-8ff0-9def9f3c6ff4"),
            OwnerId = new Guid("412dfae8-e2e0-47f3-9762-c32004c97115"),
            PetId = new Guid("da94a5b8-9a64-4e26-87fb-6ed48a6698a7")
          },
          new PetOwner
          {
            Id = new Guid("0b1d9377-47cf-49b8-8b72-a9b7db3877c4"),
            OwnerId = new Guid("4eb9c29b-d843-46d8-826e-a5821f8ac4f6"),
            PetId = new Guid("0ede9947-28af-489f-b15d-658a4df73c8f")
          }
    };
}
