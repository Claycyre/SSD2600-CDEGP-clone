using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models
{
    [Table("ContactDetail")]
    public class ContactDetail
    {
        [Key]
        public int PkContactId { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NameFirst { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NameLast { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string StreetAddress { get; set; } = string.Empty;

        [StringLength(50)]
        public string? AdministrativeArea { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
