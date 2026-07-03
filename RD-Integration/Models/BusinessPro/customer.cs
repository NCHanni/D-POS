using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class customer
    {
        [Key]
        public string code { get; set; }
        public string codename { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string contact_no { get; set; }
        public string tin { get; set; }
        public string business_style { get; set; }
        public string discount_type { get; set; }
        public string gender { get; set; }
        public DateTime? birthdate { get; set; }
        public string sc_pwd_no { get; set; }
        public DateTime? issued_date { get; set; }
        public string allow_discount { get; set; }
        public string is_active { get; set; }
        public DateTime timestamp { get; set; }
        public string default_atc { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }

        public string gen_bus_posting_group { get; set; } = string.Empty;
        public string vat_bus_posting_group { get; set; } = string.Empty;
        public string customer_price_group { get; set; } = string.Empty;
        public string customer_discount_group { get; set; } = string.Empty;
    }

    public class customer_code
    {
        [Key]
        public string code { get; set; }
    }
}
