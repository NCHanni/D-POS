using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class unit_of_measure
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
    }
}
