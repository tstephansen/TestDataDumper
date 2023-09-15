using System;
using System.Collections.Generic;

namespace TestDataDumper.Data.Entities;

public partial class Pet
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public virtual ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();
}
