using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.Data;
using Microsoft.AspNetCore.Identity;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API_Sistema_Central.Services;
using API_Sistema_Central.Repositories;
using API_Sistema_Central.Authentication;

namespace API_Sistema_Central
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_Sistema_Central", Version = "v1" });
            });*/

            services.AddDbContext<SCContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SCContext")));

            services.AddIdentity<Utilizador, IdentityRole>().AddEntityFrameworkStores<SCContext>().AddDefaultTokenProviders();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IUtilizadorService, UtilizadorService>();
            services.AddTransient<ITransacaoService, TransacaoService>();
            services.AddTransient<IReservaService, ReservaService>();
            services.AddTransient<IPagamentoService, PagamentoService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ISubAluguerService, SubAluguerService>();

            services.AddScoped<ICartaoRepository, CartaoRepository>();
            services.AddScoped<IDebitoDiretoRepository, DebitoDiretoRepository>();
            services.AddScoped<IMetodoPagamentoRepository, MetodoPagamentoRepository>();
            services.AddScoped<IParqueRepository, ParqueRepository>();
            services.AddScoped<IPayPalRepository, PayPalRepository>();
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                /*app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_Sistema_Central v1"));*/
            }

            app.UseSwagger();

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
