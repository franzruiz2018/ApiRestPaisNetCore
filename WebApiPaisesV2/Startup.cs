using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiPaisesV2.Models;

namespace WebApiPaisesV2
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
            //Servicio para agregar Entity Framework
            //Local
           //// services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("paisDB"));
            //Base de datos
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection") ));
            //Sistema de usurios por defecto 
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>().
                AddDefaultTokenProviders();
            //Configuracion Token  
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "yourdomain.com",
                     ValidAudience = "yourdomain.com",
                     IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),
                     ClockSkew = TimeSpan.Zero
                 });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(ConfigureJson);
        }

        public void ConfigureJson(MvcJsonOptions obj) {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            if (!context.Paises.Any())
            {

                context.Paises.AddRange(new List<Pais>()
                                                      {
                                                        new Pais(){ Nombre="Peru", Provincias = new List<Provincia>(){ new Provincia() { Nombre="Lima"}, new Provincia() { Nombre = "Ica" } } },
                                                        new Pais(){ Nombre="Ecuador"},
                                                        new Pais(){ Nombre="Chile"},
                                                     });
                context.SaveChanges();
            }
        }
    }
}
