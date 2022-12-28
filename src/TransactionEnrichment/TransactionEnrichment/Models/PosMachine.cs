using System;
using System.Collections.Generic;

namespace TransactionEnrichment.Models;

public partial class PosMachine
{
    public Guid Id { get; set; }

    public string ZipCode { get; set; } = null!;

    public Guid MerchantId { get; set; }

    public virtual ICollection<CreditCardTransaction> CreditCardTransactions { get; } = new List<CreditCardTransaction>();

    public virtual Merchant Merchant { get; set; } = null!;
}
