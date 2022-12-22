using System;
using System.Collections.Generic;

namespace CreditCardSimulator.Models;

public partial class CreditCardTransaction
{
    public Guid Id { get; set; }

    public Guid CreditCardId { get; set; }

    public Guid PosMachineId { get; set; }

    public double Amount { get; set; }

    public virtual CreditCard CreditCard { get; set; } = null!;

    public virtual PosMachine PosMachine { get; set; } = null!;
}
