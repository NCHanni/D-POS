using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class credit_card
    {
        [Key]
        public string code { get; set; }
        public string description { get; set; }
        public string bank_name { get; set; }
        public string account_type { get; set; }
        public string account_no { get; set; }
        public string is_active { get; set; }
    }
}
