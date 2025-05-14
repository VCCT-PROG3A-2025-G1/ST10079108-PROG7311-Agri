using AgriApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AuthDbContext dbContext;

        public EmployeesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AuthDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> ViewFarmers()
        {
            var farmers = await userManager.GetUsersInRoleAsync("Farmer");
            return View(farmers);
        }

        public async Task<IActionResult> ViewProducts(string id)
        {
            var products = await dbContext.Products.Where(prd => prd.FarmerId == id).ToListAsync();

            var farmer = await userManager.FindByIdAsync(id);
            ViewBag.FarmerEmail = farmer?.Email;

            return View(products);
        }
    }
}
