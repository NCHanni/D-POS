using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class item_unit_of_measure
    {
        [Key]
        public long id { get; set; }
        public string item_no { get; set; }
        public string unit_of_measure { get; set; }
        public decimal qty_per_uom { get; set; }
        public string to_uom { get; set; }
    }
}
