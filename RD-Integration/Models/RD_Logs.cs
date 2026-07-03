using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models
{
    public class RD_Logs
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Date/Time")]
        public DateTime LogDateTime{get; set; } = DateTime.Now;
        [Display(Name = "Log")]
        public string Message { get; set; }
    }
}
