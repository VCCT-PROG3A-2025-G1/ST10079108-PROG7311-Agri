using AgriApp.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgriApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Agri"))); //Connecting local database
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Login";
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); //Ensures that certain pages are only accessable to authorized users

            app.MapRazorPages();
            app.MapDefaultControllerRoute(); //Ensure the app uses controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //Adding roles for farmers and employees
            using (var scope = app.Services.CreateScope()) 
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Farmer", "Employee" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Adding famer
            using (var scope = app.Services.CreateScope()) 
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "Mike@mail.com";
                string password = "Mike1234@";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Farmer");
                }
            }

            //Adding famer
            using (var scope = app.Services.CreateScope()) 
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "John@mail.com";
                string password = "John1234@";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Farmer");
                }
            }

            //Adding famer
            using (var scope = app.Services.CreateScope())
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "Yui@mail.com";
                string password = "Yui1234@";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Employee");
                }
            }

            app.Run();
        }
    }
}
