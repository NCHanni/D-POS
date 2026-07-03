using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class sale_details
    {
        [Key]
        public long id { get; set; }
        public int item_line_id { get; set; }
        public string sale_code { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string class_code { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal qty_returned { get; set; }
        public string unit_of_measure { get; set; }
        public decimal qty_per_uom { get; set; }
        public string is_regular_discount { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal discounted_price { get; set; }
        public decimal vat_percent { get; set; }
        public decimal vat_amount { get; set; }
        public decimal vat_exempt_amount { get; set; }
        public decimal amount { get; set; }
        public string is_vatable { get; set; }
        public string is_zero_rated { get; set; }
        public string is_vat_exempt { get; set; }
        public string is_gift_certificate { get; set; }
        public string serial_nbr { get; set; }
        public string alternate_id { get; set; }
    }
}
