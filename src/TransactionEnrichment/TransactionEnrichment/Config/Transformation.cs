using System.ComponentModel.DataAnnotations;

namespace TransactionEnrichment.Config
{
    public class Transformation
    {
        [Required]
        public required string Source { get; init; }
        [Required]
        public required string Destination { get; init; }
    }
}
