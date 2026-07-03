using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RD_INTEGRATION.Data;

namespace RD_INTEGRATION
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //case 102
        {
            Configuration = configuration;// commit 2
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<DBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DBContext"))); //, 
                        //sqlServerOptionsAction: sqlOptions =>
                        //{
                        //    sqlOptions.EnableRetryOnFailure(
                        //        maxRetryCount: 10,
                        //        maxRetryDelay: TimeSpan.FromSeconds(30),
                        //        errorNumbersToAdd: null
                        //        );
                        //}));

            services.AddHostedService<Services.ScheduleHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
