using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class item_class
    {
        [Key]
        public string code { get; set; }
        public string codename { get; set; }
        public string description { get; set; }
    }
}
