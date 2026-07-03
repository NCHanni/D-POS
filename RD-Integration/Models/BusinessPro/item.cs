using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class item
    {
        [Key]
        public string code { get; set; }
        public string codename { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string specifications { get; set; }
        public string class_code { get; set; }
        public string unit_of_measure { get; set; }
        public decimal price { get; set; }
        public string is_vat { get; set; }
        public string is_zero_rated { get; set; }
        public string is_senior_pwd { get; set; }
        public string discount_group { get; set; }
        public string allow_discount { get; set; }
        public string inventory_posting_group { get; set; }
        public string vat_bus_posting_group { get; set; }
        public string gen_prod_posting_group { get; set; }
        public string vat_prod_posting_group { get; set; }
        public string is_active { get; set; }
        public string is_lot { get; set; }
        public string is_serial { get; set; }
    }
}
