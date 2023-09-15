using TestDataDumper.Data.Entities;

namespace TestDataDumper.TestData;

public static partial class AddressData
{
    public static List<Address> LoadAddresses() => new List<Address>
    {
        new Address
        {
            Id = new Guid("e4fd2b47-8d09-4c17-ad4c-26ef61b2de17"),
            Street = "123 Main Street",
            Street2 = "Suite 100",
            City = "Johns Creek",
            State = "GA",
            Zip = "30022",
            Country = "USA"
        },
        new Address
        {
            Id = new Guid("59f1f53b-5af5-434e-9215-581cbe98b106"),
            Street = "456 Second Ave.",
            Street2 = null,
            City = "Roswell",
            State = "GA",
            Zip = "30076",
            Country = "USA"
        },
        new Address
        {
            Id = new Guid("887257ed-a55c-4837-bf23-ac2c74f6c65a"),
            Street = "789 Third Street NE",
            Street2 = "Apt. 210",
            City = "Atlanta",
            State = "GA",
            Zip = "30319",
            Country = "USA"
        }
    };
}