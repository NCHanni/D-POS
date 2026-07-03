using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models
{
    public class RD_Error
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Date/Time")]
        public DateTime ErrorDateTime { get; set; } = DateTime.Now;
        [Display(Name = "Source")]
        public string Source { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
