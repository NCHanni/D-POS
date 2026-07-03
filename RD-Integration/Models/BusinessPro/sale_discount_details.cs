using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class sale_discount_details
    {
        [Key]
        public long id { get; set; }
        public string sale_code { get; set; }
        public string discount_type { get; set; }
        public string id_no { get; set; }
        public string name { get; set; }
        public string gender{ get; set; }
        public DateTime birthdate { get; set; }
        public DateTime issued_date { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_less_vat { get; set; }
    }
}
