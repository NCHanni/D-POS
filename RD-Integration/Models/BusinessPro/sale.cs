using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class sale
    {
        [Key]
        public string code { get; set; }
        public long cashier_session_id { get; set; }
        public string terminal_code { get; set; }
        public DateTime transaction_date { get; set; }
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string customer_tin { get; set; }
        public string customer_business_style { get; set; }
        public string customer_address { get; set; }
        public string salesperson_code { get; set; }
        public string salesperson { get; set; }
        public string cashier_code { get; set; }
        public string cashier_name { get; set; }
        public string invoice_code { get; set; }
        public string payment_code { get; set; }
        public decimal total_vat { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal total_amount_returned { get; set; }
        public decimal vat_sales { get; set; }
        public decimal vat_exempt_sales { get; set; }
        public decimal zero_rated_sales { get; set; }
        public decimal less_vat { get; set; }
        public string is_sc { get; set; }
        public string is_pwd { get; set; }
        public string sc_pwd_id_no { get; set; }
        public string sc_pwd_name { get; set; }
        public decimal sc_pwd_discount { get; set; }
        public decimal sc_pwd_less_vat { get; set; }
        public string is_void { get; set; }
        public string void_code { get; set; }
        public string is_all_returned { get; set; }
        public string is_synced { get; set; }
        public string acu_batch_ref_number { get; set; }
    }
}
