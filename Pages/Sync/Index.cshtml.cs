using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RD_INTEGRATION.Models;
using RD_INTEGRATION.Models.BusinessPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Pages.Sync
{
    public class IndexModel : PageModel
    {
        private readonly RD_INTEGRATION.Data.DBContext _context;

        public IndexModel(RD_INTEGRATION.Data.DBContext context)
        {
            _context = context;
        }

        public RD_Preference preference { get; set; }
        public IList<settings_sync> settings_sync { get; set; }

        public async Task OnGetAsync()
        {
            // 3809 : POS Memo - Hid credit memo table from sync options.
            settings_sync = await _context.settings_sync.Where(s => s.rd_integration == true).OrderBy(s => s.sort_order).ToListAsync();
            preference = await _context.rd_preference.SingleOrDefaultAsync();
        }

        public async Task OnPostRun(string tableName)
        {
            switch (tableName)
            {
                case "company_information":
                    await new Services.SyncCompany(_context).Run(true);
                    break;
                case "credit_card":
                    await new Services.SyncCreditCard(_context).Run(true);
                    break;
                case "credit_memo":
                    await new Services.SyncCreditMemo(_context).Run(true);
                    break;
                case "customer":
                    await new Services.SyncCustomer(_context).Run(true);
                    break;
                case "location":
                    await new Services.SyncLocation(_context).Run(true);
                    break;
                case "gift_certificate":
                    await new Services.SyncGiftCert(_context).Run(true);
                    break;
                case "salesperson":
                    await new Services.SyncSalesperson(_context).Run(true);
                    break;
                case "unit_of_measure":
                    await new Services.SyncUOM(_context).Run(true);
                    break;
                case "discount_group":
                    await new Services.SyncDiscount(_context).Run(true);
                    break;
                case "item_class":
                    await new Services.SyncItemClass(_context).Run(true);
                    break;
                case "item_sale_price":
                    await new Services.SyncItemSalePrice(_context).Run(true);
                    break;
                case "item":
                    await new Services.SyncItem(_context).Run(true);
                    break;
                case "item_barcode":
                    await new Services.SyncBarcode(_context).Run(true);
                    break;
                case "item_unit_of_measure":
                    await new Services.SyncItemUOM(_context).Run(true);
                    break;
                case "sale":
                    await new Services.SyncSale(_context).Run(true);
                    break;
                default:
                    break;
            }

            await OnGetAsync();
        }
    }
}
