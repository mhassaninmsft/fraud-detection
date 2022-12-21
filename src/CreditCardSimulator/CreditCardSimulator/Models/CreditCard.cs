using System;
using System.Collections.Generic;

namespace CreditCardSimulator.Models;

public partial class CreditCard
{
    public Guid Id { get; set; }

    public string CardNumber { get; set; } = null!;

    public string IssuingBank { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public int ExpirationMonth { get; set; }

    public int ExpirationYear { get; set; }

    public virtual ICollection<CreditCardTransaction> CreditCardTransactions { get; } = new List<CreditCardTransaction>();
}
