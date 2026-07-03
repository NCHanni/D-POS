using Microsoft.EntityFrameworkCore;
using RD_INTEGRATION.Models.BusinessPro;

namespace RD_INTEGRATION.Data
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<credit_memo>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<discount_group>().Property(p => p.discount_rate).HasColumnType("numeric(4,2)");
            builder.Entity<gift_certificate>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<item>().Property(p => p.price).HasColumnType("numeric(12,2)");
            builder.Entity<item_sale_price>().Property(p => p.minimum_quantity).HasColumnType("numeric(8,2)");
            builder.Entity<item_sale_price>().Property(p => p.unit_price).HasColumnType("numeric(8,2)");
            builder.Entity<item_unit_of_measure>().Property(p => p.qty_per_uom).HasColumnType("numeric(12,2)");
            builder.Entity<payment_cash>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<payment_cash>().Property(p => p.change).HasColumnType("numeric(18,2)");
            builder.Entity<payment_credit_card>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<payment_credit_memo>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<payment_gift_certificate>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<sale>().Property(p => p.less_vat).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.sc_pwd_discount).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.sc_pwd_less_vat).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.total_amount).HasColumnType("numeric(18,2)");
            builder.Entity<sale>().Property(p => p.total_amount_returned).HasColumnType("numeric(18,2)");
            builder.Entity<sale>().Property(p => p.total_discount).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.total_vat).HasColumnType("numeric(18,2)");
            builder.Entity<sale>().Property(p => p.vat_exempt_sales).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.vat_sales).HasColumnType("numeric(18,8)");
            builder.Entity<sale>().Property(p => p.zero_rated_sales).HasColumnType("numeric(18,8)");
            builder.Entity<sale_details>().Property(p => p.amount).HasColumnType("numeric(18,2)");
            builder.Entity<sale_details>().Property(p => p.discount_amount).HasColumnType("numeric(18,8)");
            builder.Entity<sale_details>().Property(p => p.discount_percent).HasColumnType("numeric(5,2)");
            builder.Entity<sale_details>().Property(p => p.discounted_price).HasColumnType("numeric(18,2)");
            builder.Entity<sale_details>().Property(p => p.price).HasColumnType("numeric(18,2)");
            builder.Entity<sale_details>().Property(p => p.qty).HasColumnType("numeric(18,2)");
            builder.Entity<sale_details>().Property(p => p.qty_per_uom).HasColumnType("numeric(10,2)");
            builder.Entity<sale_details>().Property(p => p.qty_returned).HasColumnType("numeric(18,2)");
            builder.Entity<sale_details>().Property(p => p.vat_amount).HasColumnType("numeric(18,8)");
            builder.Entity<sale_details>().Property(p => p.vat_exempt_amount).HasColumnType("numeric(18,8)");
            builder.Entity<sale_details>().Property(p => p.vat_percent).HasColumnType("numeric(5,2)");
            builder.Entity<sale_discount_details>().Property(p => p.total_discount).HasColumnType("numeric(18,8)");
            builder.Entity<sale_discount_details>().Property(p => p.total_less_vat).HasColumnType("numeric(18,8)");
        }

        public DbSet<Models.RD_Preference> rd_preference { get; set; }
        public DbSet<Models.RD_Error> rd_error { get; set; }
        public DbSet<Models.RD_Logs> rd_logs { get; set; }

        public DbSet<settings_sync> settings_sync { get; set; }
        public DbSet<settings_preference> settings_preference { get; set; }
        public DbSet<company_information> company_information { get; set; }
        public DbSet<credit_card> credit_card { get; set; }
        public DbSet<credit_memo> credit_memo { get; set; }
        public DbSet<customer> customer { get; set; }
        public DbSet<customer_code> customer_code { get; set; }
        public DbSet<location> location { get; set; }
        public DbSet<gift_certificate> gift_certificate { get; set; }
        public DbSet<salesperson> salesperson { get; set; }
        public DbSet<unit_of_measure> unit_of_measure { get; set; }
        public DbSet<discount_group> discount_group { get; set; }
        public DbSet<item_class> item_class { get; set; }
        public DbSet<item_sale_price> item_sale_price { get; set; }
        public DbSet<item> item { get; set; }
        public DbSet<item_barcode> item_barcode { get; set; }
        public DbSet<item_unit_of_measure> item_unit_of_measure { get; set; }

        //Sale
        public DbSet<sale> sale { get; set; }
        public DbSet<sale_details> sale_details { get; set; }
        public DbSet<sale_discount_details> sale_discount_details { get; set; }
        public DbSet<payment_cash> payment_cash { get; set; }
        public DbSet<payment_credit_card> payment_credit_card { get; set; }
        public DbSet<payment_credit_memo> payment_credit_memo { get; set; }
        public DbSet<payment_gift_certificate> payment_gift_certificate { get; set; }
    }
}
