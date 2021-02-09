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

namespace API_Sistema_Central
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_Sistema_Central", Version = "v1" });
            });

            services.AddDbContext<SCContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SCContext")));

            services.AddIdentity<Utilizador, IdentityRole>()
                .AddEntityFrameworkStores<SCContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                });

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUtilizadorService, UtilizadorService>();
            services.AddTransient<ICartaoService, CartaoService>();
            services.AddTransient<IReservaService, ReservaService>();

            services.AddScoped<ICartaoRepository, CartaoRepository>();
            services.AddScoped<IDebitoDiretoRepository, DebitoDiretoRepository>();
            services.AddScoped<IMetodoPagamentoRepository, MetodoPagamentoRepository>();
            services.AddScoped<IParqueRepository, ParqueRepository>();
            services.AddScoped<IPayPalRepository, PayPalRepository>();
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_Sistema_Central v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
