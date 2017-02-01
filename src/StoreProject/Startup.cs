using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoreProject.Models;
using StoreProject.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.AspNet.Authentication.Facebook;

namespace StoreProject
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()              
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
             
        
       

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddTransient<SampleData>();
            ////Facebook
            //var appEnv = services.BuildServiceProvider().GetRequiredService<IApplicationEnvironment>();
            //var configurationPath = Path.Combine(appEnv.ApplicationBasePath, "config.json");
            //var configBuilder = new ConfigurationBuilder().AddJsonFile(configurationPath);
            //var configuration = configBuilder.Build();

            //services.Configure<FacebookAuthenticationOptions>(options =>
            //{
            //    options.AppId = Configuration["Authentication:Facebook:AppID"];
            //    options.AppSecret = Configuration["Authentication: Facebook:AppSecret"];
            //    options.Scope.Add("public_profile");
            //    options.Scope.Add("user_birthday");
            //    options.Scope.Add("email");
            //    options.Notifications = new FacebookAuthenticationNotifications
            //    {
            //        OnAuthenticated = async context =>
            //        {
            //            var identity = (System.Security.Claims.ClaimsIdentity)context.Principal.Identity;
            //            identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
            //        }
            //    };
            //});
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SampleData seeder, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            //seeder.EnsureSeedData();
            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            
            


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            await CreateRoles(serviceProvider);
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var admin = new ApplicationUser { UserName = "admin@gmail.com", Email = "admin@gmail.com" };
            var create = await UserManager.CreateAsync(admin, "Aa123456!");
            if (create.Succeeded)
            {
                await UserManager.AddToRoleAsync(admin, "Admin");
            }
            var user = await UserManager.FindByIdAsync("ee3d361f-57f9-42f1-ac5d-b3b5fdbd69e1");
            await UserManager.AddToRoleAsync(user, "Admin");
        }
        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }


}
