using System;
using System.Collections.Generic;

namespace CreditCardSimulator.Models;

public partial class PosMachine
{
    public Guid Id { get; set; }

    public string ZipCode { get; set; } = null!;
}
