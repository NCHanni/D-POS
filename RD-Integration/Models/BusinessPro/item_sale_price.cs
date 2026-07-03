using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class item_sale_price
    {
        [Key]
        public long id { get; set; }
        public string sales_type { get; set; }
        public string sales_code { get; set; }
        public string item_no { get; set; }
        public string unit_of_measure_code { get; set; }
        public decimal minimum_quantity { get; set; }
        public DateTime? starting_date { get; set; }
        public DateTime? ending_date { get; set; }
        public decimal unit_price { get; set; }
        public bool price_includes_vat { get; set; }
        public string alternate_id { get; set; }
    }
}
