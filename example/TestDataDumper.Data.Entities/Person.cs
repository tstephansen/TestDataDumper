using System;
using System.Collections.Generic;

namespace TestDataDumper.Data.Entities;

public partial class Person
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public Guid? AddressId { get; set; }

    public virtual Address? Address { get; set; }
    public virtual ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();
}
