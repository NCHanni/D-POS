using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class settings_sync
    {
        [Key]
        [ReadOnly(true)]
        [Display(Name = "Table Name")]
        public string table_name { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Last Sync")]
        public DateTime? timestamp { get; set; }

        [Display(Name = "Enable")]
        public bool enabled { get; set; }

        [Display(Name = "Sort")]
        public byte sort_order { get; set; }
        
        [Display(Name = "Cron")]
        public string cron { get; set; }
        
        public bool rd_integration { get; set; }
    }
}
