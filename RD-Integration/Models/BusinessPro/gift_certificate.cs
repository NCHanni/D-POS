using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class gift_certificate
    {
        [Key]
        public long id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public DateTime? expiry_date { get; set; }
        public decimal amount { get; set; }
        public string is_active { get; set; }
        public string is_sold { get; set; }
        public string is_used { get; set; }
        public DateTime timestamp { get; set; }
    }
}
