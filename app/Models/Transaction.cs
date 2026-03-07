using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models
{
    [Table("OrderTransaction")]
    public class Transaction
    {
        [Key]
        public int PkTransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string GatewayTransactionId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string GatewayName { get; set; } = string.Empty;

        [Required]
        public double Subtotal { get; set; }

        public double? CombinedTax { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
