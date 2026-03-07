using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models
{
    [Table("ProductOrder")]
    public class Order
    {
        [Key]
        public int PkOrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string FkUserId { get; set; } = string.Empty;

        public int? FkTransactionId { get; set; }

        [ForeignKey(nameof(FkUserId))]
        public ApplicationUser? User { get; set; }

        [ForeignKey(nameof(FkTransactionId))]
        public Transaction? Transaction { get; set; }

        public ICollection<OrderLineItem> OrderLineItems { get; set; } = [];
    }
}
