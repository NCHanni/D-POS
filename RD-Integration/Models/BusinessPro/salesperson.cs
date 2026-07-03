using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class salesperson
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string barcode { get; set; }
    }
}
