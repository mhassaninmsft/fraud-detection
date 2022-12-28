using System.ComponentModel.DataAnnotations;

namespace TransactionEnrichment.Config
{
    public class Database
    {
        [Required]
        public required string ConnectionString { get; init; }
    }
}
