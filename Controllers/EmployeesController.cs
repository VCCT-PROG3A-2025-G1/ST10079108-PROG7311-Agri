using AgriApp.Models;
using AgriApp.Models.Entities;
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

        [HttpGet]
        public async Task<IActionResult> ViewProducts(string id, string str)
        {
            //var products = await dbContext.Products.Where(prd => prd.FarmerId == id).ToListAsync();

            IQueryable<Product> query = dbContext.Products.Where(prd => prd.FarmerId == id);
            
            var farmer = await userManager.FindByIdAsync(id);
            ViewBag.FarmerEmail = farmer?.Email;
            ViewBag.FarmerId = id;

            if (string.IsNullOrEmpty(str))
            {
                // no filter
            }
            else if (str == "Name")
            {
                query = query.OrderBy(prd => prd.Name);
            }
            else if (str == "Date")
            {
                query = query.OrderBy(prd => prd.Date);
            }
            else if (str == "Category")
            {
                query = query.OrderBy(prd => prd.Category);
            }


            var products = await query.ToListAsync();

            

            return View(products);
        }
    }
}
