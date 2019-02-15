using Drugstore.Data;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases;
using Drugstore.UseCases.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Drugstore
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
            services.AddDbContext<DrugstoreDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:WarehouseConnection"]));
            services.AddIdentity<SystemUser, IdentityRole>()
                .AddEntityFrameworkStores<DrugstoreDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ICopy, FileCopy>();
            services.AddTransient<ISerializer<MemoryStream, XmlMedicineSupplyModel>, XmlMedicineSerializer>();

            UseCaseDependencyResolver.Resolve(services);
            MapperDependencyResolver.Resolve();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Account/Login";
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });
            services.AddMvc();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    // Insert data in database
                    DataSeeder.InitializeDepartments(scope.ServiceProvider);
                    DataSeeder.InitializeUsers(scope.ServiceProvider);
                    DataSeeder.InitializeMedicine(scope.ServiceProvider);

                    #region Not validated seed
                    //DataSeeder.InitializeExternalDrugstoreMedicines(scope.ServiceProvider);
                    //DataSeeder.InitializeExternalDrugstoreSoldMedicines(scope.ServiceProvider);
                    //DataSeeder.InitializePresciptions(scope.ServiceProvider);
                    #endregion
                }
            }

            loggerFactory.AddFile("Logs/drugstore-{Date}.txt", LogLevel.Warning);
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller}/{action}",
                    new {controller = "Account", action = "Redirect"}
                );
            });
        }
    }
}