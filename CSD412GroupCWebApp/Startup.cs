using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using CSD412GroupCWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CSD412GroupCWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using LinqToTwitter;

namespace CSD412GroupCWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static SingleUserInMemoryCredentialStore TwitterCredentials { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddCors();
            services.AddRazorPages();

            services.AddControllersWithViews(config => 
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            TwitterCredentials = Configuration.GetSection("Twitter").Get<SingleUserInMemoryCredentialStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

            // Where are you going?
            app.UseRouting();

            // Who are you?
            app.UseAuthentication();

            // Are you allowed to go there?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            
            InitializeOwnerUser(userManager, Constants.OwnerEmailAddress).Wait();

            InitializeRoles(roleManager, Constants.RoleNames).Wait();
            InitializeOwnerUserRoles(userManager, Constants.RoleNames, Constants.OwnerEmailAddress).Wait();
        }

        private async Task<ApplicationUser> InitializeOwnerUser(UserManager<ApplicationUser> userManager, string ownerEmailAddress)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == ownerEmailAddress);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Name = "Site Owner",
                    Email = ownerEmailAddress,
                    NormalizedEmail = ownerEmailAddress.ToUpper(),
                    UserName = ownerEmailAddress,
                    NormalizedUserName = ownerEmailAddress.ToUpper(),
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                string defaultOwnerPassword = "Secret1!";
                await userManager.CreateAsync(user, defaultOwnerPassword);
            }

            return user;
        }

        private async Task InitializeRoles(RoleManager<IdentityRole> roleManager, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                if (!roleManager.Roles.Any(r => r.Name == roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task InitializeOwnerUserRoles(UserManager<ApplicationUser> userManager, string[] roleNames, string ownerEmailAddress)
        {
            ApplicationUser user = await userManager.FindByNameAsync(ownerEmailAddress);
            await userManager.AddToRolesAsync(user, roleNames);
        }
    }
}
