using menus_project.Data;
using Microsoft.AspNetCore.Mvc;
using menus_project.ViewModel;
using menus_project.Models;
using menus_project.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using menus_project.Constants;
using Microsoft.EntityFrameworkCore;

namespace menus_project.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = AppRoles.RestaurantOwner)]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
		private readonly Helper _helpers;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(AppDbContext context, Helper helper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _helpers = helper;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Restaurant restaurant = _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .Include(r => r.MenuItems)
                .Include(r => r.Locations).FirstOrDefault(r => r.UserId == _userManager.GetUserId(User));

            return View(restaurant);
        }

        public IActionResult Reviews()
        {
            List<RestaurantReview> reviews = _context.RestaurantReviews.Include(r=> r.User).Where(r=> r.UserId == _userManager.GetUserId(User) ).ToList();
            return View(reviews);
        }

        [HttpGet]
        public IActionResult Create(string searchTerm) 
        {
            var itemsQuery = _context.MenuItems.Include(x => x.MenuItemCategory).AsQueryable();

            if(!String.IsNullOrEmpty(searchTerm))
            {
                itemsQuery = itemsQuery.Where(m => (m.NameInArabic.Contains(searchTerm)) || (m.Name.Contains(searchTerm)));
				ViewBag.CurrentSearch = searchTerm;

			}
            var vm = new MenuItemViewModel
            {
                Categories = _context.MenuItemCategories.ToList(),
				Items = itemsQuery.ToList(),
                Item = new MenuItem()
			};
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItemViewModel menuItem)   
        {
			string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (menuItem.Item.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Validation.Required.Image");
            else
            {
                string extension = Path.GetExtension(menuItem.Item.ImageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    ModelState.AddModelError("ImageFile", "Validation.InvalidImageType");
            }

            if (ModelState.IsValid)
            {

				var user = await _context.Users.Include(x => x.Restaurant).FirstOrDefaultAsync(x => x.Id == _userManager.GetUserId(User));
                int restaurantId = user.Restaurant.Id;
				MenuItem item = new MenuItem
                {
                    Name = menuItem.Item.Name,
                    NameInArabic = menuItem.Item.NameInArabic,
                    Description = menuItem.Item.Description,
                    DescriptionInArabic = menuItem.Item.DescriptionInArabic,
                    MenuItemCategoryId = menuItem.Item.MenuItemCategoryId,
                    Price = menuItem.Item.Price,
                    Image_url = _helpers.SaveImage(menuItem.Item.ImageFile),
                    RestaurantId = restaurantId,
                    
                };

                _context.MenuItems.Add(item);
                _context.SaveChanges();

                return RedirectToAction("Create");
            }

			menuItem.Categories = _context.MenuItemCategories.ToList();
			menuItem.Items = _context.MenuItems.ToList();
			return View("Create",menuItem);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var menuItem = _context.MenuItems.Find(id);

            if (menuItem == null)
                return NotFound();


            var vm = new MenuItemViewModel
			{
				Categories = _context.MenuItemCategories.ToList(),
				Item = menuItem
			};

			return View(vm);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public  async Task<IActionResult> Edit(MenuItemViewModel menuItem) 
        {
            if(menuItem.Item.ImageFile != null)
            {
				menuItem.Item.Image_url = _helpers.SaveImage(menuItem.Item.ImageFile);
			}

			var user = await _context.Users.Include(x => x.Restaurant).FirstOrDefaultAsync(x => x.Id == _userManager.GetUserId(User));
			int restaurantId = user.Restaurant.Id;
            menuItem.Item.RestaurantId = restaurantId;



			if (!ModelState.IsValid)
                return View(menuItem);
            
            _context.MenuItems.Update(menuItem.Item);
            _context.SaveChanges();

            return RedirectToAction("Create");
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ChangeStatus(int id)
        {
            var item = _context.MenuItems.Find(id);
            
            if(item == null)
                return NotFound();

            item.IsAvailable = !item.IsAvailable;

            _context.MenuItems.Update(item);
            _context.SaveChanges();

            return RedirectToAction("Create");
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
            var userId = _userManager.GetUserId(User);
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
			var item = await _context.MenuItems.Include(m=> m.Reviews).FirstOrDefaultAsync(m => m.Id == id && m.RestaurantId == restaurant.Id);

			if (item == null)
				return NotFound();


            _helpers.DeleteImage(item.Image_url);

            _context.MenuItems.Remove(item);
			_context.SaveChanges();

			return RedirectToAction("Create");
		}


	}
}
