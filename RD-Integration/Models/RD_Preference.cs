using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models
{
    public class RD_Preference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Base URL")]
        public string BaseURL { get; set; }

        [Required]
        [Display(Name = "Endpoint Name")]
        public string EndpointName { get; set; }

        [Required]
        [Display(Name = "Endpoint Version")]
        public string EndpointVersion { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Tenant { get; set; }
        public string Branch { get; set; }
        public string Locale { get; set; }

        [Display(Name = "Transaction Date Offset")]
        public int TransactionDateOffset { get; set; }

        [Display(Name = "Timestamp Date Offset")]
        public int TimestampDateOffset { get; set; }

        [Display(Name = "Enable Schedule")]
        public bool EnableSchedule { get; set; }
    }
}
