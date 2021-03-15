using APP_FrontEnd.Data;
using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FrontEndContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<Utilizador, IdentityRole>().AddEntityFrameworkStores<FrontEndContext>().AddDefaultUI().AddDefaultTokenProviders();

            services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = "100586914365-lev9iklgg4cc7au8kl85159mms4i0vlq.apps.googleusercontent.com";
                options.ClientSecret = "Y3FUI6I4rYRVmvwMXm260Pzi";
            })
            .AddFacebook(options =>
            {
                options.AppId = "761440444771358";
                options.AppSecret = "3a45d5c169676ebcd6d0031805d1632b";
            });

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var cultureInfo = new CultureInfo("pt-PT");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
                var supportedCultures = new List<CultureInfo> { cultureInfo };
                opts.DefaultRequestCulture = new RequestCulture("pt-PT");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddMemoryCache();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddTransient<ITransacaoService, TransacaoService>();
            services.AddTransient<IUtilizadorService, UtilizadorService>();
            services.AddTransient<IReservaService, ReservaService>();
            services.AddTransient<ISubAluguerService, SubAluguerService>();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
