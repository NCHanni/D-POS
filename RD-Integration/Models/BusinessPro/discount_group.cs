using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class discount_group
    {
        [Key]
        public string code { get; set; }
        public string description { get; set; }
        public string is_vat_exempt { get; set; }
        public decimal discount_rate { get; set; }
    }
}
