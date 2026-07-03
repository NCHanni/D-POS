using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class item_barcode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string item_no { get; set; }
        public string unit_of_measure { get; set; }
        public string barcode { get; set; }
    }
}
