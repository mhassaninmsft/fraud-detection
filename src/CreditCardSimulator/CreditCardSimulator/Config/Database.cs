using System.ComponentModel.DataAnnotations;

namespace CreditCardSimulator.Config
{
    public class Database
    {
        [Required]
        public required string ConnectionString { get; init; }
    }
}
