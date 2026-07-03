using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class settings_preference
    {
        [Key]
        [ReadOnly(true)]
        public string flag { get; set; }

        [ReadOnly(true)]
        public string description { get; set; }

        [ReadOnly(true)]
        public string value { get; set; }
    }
}
