using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class company_information
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string name_2 { get; set; }
        public string permit_number { get; set; }
        public string permit_min { get; set; }
        public DateTime? permit_issued { get; set; }
        public DateTime? permit_expiry { get; set; }
        public string vat_registration_no { get; set; }
        public string business_style { get; set; }
        public string address { get; set; }
        public string contact_no { get; set; }
        public int line_limit { get; set; }
        public string fax_no { get; set; }
        public string email_address { get; set; }
        public string website_url { get; set; }
    }
}
