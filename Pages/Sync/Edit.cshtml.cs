using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RD_INTEGRATION.Models;
using RD_INTEGRATION.Models.BusinessPro;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Pages.Sync
{
    public class EditModel : PageModel
    {
        private readonly RD_INTEGRATION.Data.DBContext _context;

        public EditModel(RD_INTEGRATION.Data.DBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public settings_sync settings_sync { get; set; }
        public RD_Preference preference { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            settings_sync = await _context.settings_sync.FirstOrDefaultAsync(m => m.table_name == id);
            preference = await _context.rd_preference.SingleOrDefaultAsync();

            ViewData["CronLink"] = settings_sync.cron.Replace(" ", "_");

            if (settings_sync == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(settings_sync).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!settings_syncExists(settings_sync.table_name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool settings_syncExists(string id)
        {
            return _context.settings_sync.Any(e => e.table_name == id);
        }
    }
}
