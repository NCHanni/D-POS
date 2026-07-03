using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RD_INTEGRATION.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RD_INTEGRATION.Data.DBContext _context;
        //private readonly ILogger<IndexModel> _logger;

        public IndexModel(RD_INTEGRATION.Data.DBContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            //_logger = logger;
        }

        public IList<RD_Logs> logs { get; set; }
        public IList<RD_Error> errors { get; set; }

        public async Task OnGetAsync()
        {
            logs =
                await _context.rd_logs
                    .Where(l => l.LogDateTime >= System.DateTime.Today.AddDays(-14))
                    .OrderByDescending(l => l.LogDateTime)
                    .Take(100)
                    .ToListAsync();

            errors =
                await _context.rd_error
                    .Where(e => e.ErrorDateTime >= System.DateTime.Today.AddDays(-14))
                    .OrderByDescending(e => e.ErrorDateTime)
                    .Take(100)
                    .ToListAsync();
        }
    }
}
