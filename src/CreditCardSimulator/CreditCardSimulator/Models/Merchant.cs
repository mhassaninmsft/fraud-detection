using System;
using System.Collections.Generic;

namespace CreditCardSimulator.Models;

public partial class Merchant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PosMachine> PosMachines { get; } = new List<PosMachine>();
}
