using AgriApp.Models;
using AgriApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriApp.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmersController : Controller
    {
        private readonly AuthDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public FarmersController(AuthDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel viewModel)
        {
            var farmerID = userManager.GetUserId(User);
            var product = new Product
            {

                Name = viewModel.Name,
                Category = viewModel.Category,
                FarmerId = farmerID,
                Date = DateTime.Now,

            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(string str)
        {
            var farmerID = userManager.GetUserId(User);

            IQueryable<Product> query = dbContext.Products.Where(prd => prd.FarmerId == farmerID);

            if (string.IsNullOrEmpty(str))
            {
                // no filter
            }
            else if(str == "Name")
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


            var product = await query.ToListAsync();

            return View(product);
        }

       
    }
}
