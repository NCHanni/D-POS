using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RD_INTEGRATION.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Pages
{
    public class PreferenceModel : PageModel
    {
        private readonly RD_INTEGRATION.Data.DBContext _context;

        public PreferenceModel(RD_INTEGRATION.Data.DBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RD_Preference Preference { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                id = 1;
            }

            try
            {
                Preference = await _context.rd_preference.FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message;
            }

            if (Preference == null)
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

            _context.Attach(Preference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenceExists(Preference.Id))
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

        private bool PreferenceExists(int id)
        {
            return _context.rd_preference.Any(e => e.Id == id);
        }
    }
}
