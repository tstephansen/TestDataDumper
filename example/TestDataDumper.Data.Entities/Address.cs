using System;
using System.Collections.Generic;

namespace TestDataDumper.Data.Entities;

public partial class Address
{
    public Guid Id { get; set; }

    public string Street { get; set; } = null!;

    public string? Street2 { get; set; }

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public string? Zip { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
