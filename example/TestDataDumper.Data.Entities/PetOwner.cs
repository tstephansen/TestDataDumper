using System;
using System.Collections.Generic;

namespace TestDataDumper.Data.Entities;

public partial class PetOwner
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    public Guid PetId { get; set; }

    public virtual Person Owner { get; set; } = null!;

    public virtual Pet Pet { get; set; } = null;
}
