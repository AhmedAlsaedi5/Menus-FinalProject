using menus_project.Constants;
using menus_project.Data;
using menus_project.Helpers;
using menus_project.Models;
using menus_project.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace menus_project.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;

		}
		public async Task<IActionResult> Index(int? id)
		{
			var restaurantsQuery = _context.Restaurants.Include(r => r.RestaurantCategories)
			.Include(r => r.Locations)
			.AsQueryable();

			if (id != null)
			{
				restaurantsQuery = restaurantsQuery.Where(r =>r.RestaurantCategories.Any(c => c.Id == id));
			}


			var favoriteIds = new List<int>();

			if (User.Identity.IsAuthenticated)
			{
				var user = await _context.Users
					.Include(u => u.FavoriteRestaurants)
					.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

				if (user != null)
				{
					favoriteIds = user.FavoriteRestaurants.Select(r => r.Id).ToList();
				}
			}
			var vm = new HomeViewModel
			{
				Restaurants = restaurantsQuery.ToList(),
				RestaurantsCategory = _context.RestaurantCategories.ToList(),
				FavoriteRestaurantIds = favoriteIds
				
			};


			
			return View(vm);
		}

        [Authorize(Roles = AppRoles.User + "," + AppRoles.RestaurantOwner)]
        [HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> ChangeFavorite(int id)
		{
			var user = await _context.Users.Include(u => u.FavoriteRestaurants)
				.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

			var restaurant = _context.Restaurants.Find(id);

			if (restaurant == null)
				return NotFound();

			if (user.FavoriteRestaurants.Any(r => r.Id == id))
			{
				user.FavoriteRestaurants.Remove(restaurant);
			}
			else
			{
				user.FavoriteRestaurants.Add(restaurant);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}


		public async Task< IActionResult> Menu(int id)
		{
			var menu = _context.MenuItems.Include(m => m.Restaurant)
				.Include(m => m.MenuItemCategory)
				.Where(m => m.RestaurantId == id && m.IsAvailable)
				.ToList();

			var restaurant = _context.Restaurants.Include(r=> r.RestaurantCategories).Where(r => r.Id == id).FirstOrDefault();

			var user = await _context.Users.Include(u => u.FavoriteRestaurants)
				.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

			bool isFavorite = false;

			if (user != null)
				 isFavorite = user.FavoriteRestaurants.Any(r=> r.Id == id);

			var categories = menu.Select(m => m.MenuItemCategory).Distinct().ToList();
		

			var vm = new MenuViewModel
			{
				Restaurant = restaurant,
				MenuItems = menu,
				IsFavoriteRestaurant = isFavorite,
				MenuItemCategories = categories,
				Review = new MenuItemReview()
			};

			return View(vm);
		}

		[Route("reviews")]
		public IActionResult Reviews(int id)
		{
			var restaurantReviews = _context.RestaurantReviews.Include(r => r.User)
				.Where(r => r.RestaurantId == id)
				.ToList();

			var restaurant = _context.Restaurants.Include(r => r.RestaurantCategories)
				.Where(r=> r.Id == id)
				.FirstOrDefault();

			bool isReviewed = false;

			if (User.Identity.IsAuthenticated)
			{
				var userId = _userManager.GetUserId(User);

				isReviewed = _context.RestaurantReviews.Any(r => r.UserId == userId && r.RestaurantId == id);
			}


			var vm = new RestaurantReviewViewModel
			{
				RestaurantReviews = restaurantReviews,
				Restaurant = restaurant,
				IsReviewed = isReviewed,
				RestaurantId = id,
				AddRestaurantReview = new RestaurantReview()
			};

			return View(vm);
		}


        [Authorize(Roles = AppRoles.User)]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddRestaurantReview(RestaurantReviewViewModel review)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

			if (user == null)
				return Unauthorized();

			if (review.RestaurantId <= 0)
				return BadRequest();


			var userReview = new RestaurantReview
			{
				UserId = user.Id,
				RestaurantId = review.RestaurantId,
				Comment = review.AddRestaurantReview.Comment,
				Rating = review.AddRestaurantReview.Rating,
				CreatedAt = DateTime.Now
			};

			_context.RestaurantReviews.Add(userReview);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Reviews), new { id = review.RestaurantId });
		}


        [Authorize(Roles = AppRoles.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewItem(MenuItemReview review)
        {
            var restaurantId = await _context.MenuItems
                .Where(m => m.Id == review.MenuItemId)
                .Select(m => m.RestaurantId)
                .FirstOrDefaultAsync();

            if (restaurantId == 0)
                return NotFound();

            bool alreadyReviewed = await _context.MenuItemReviews.AnyAsync(r =>
			   r.UserId == _userManager.GetUserId(User) &&
			   r.MenuItemId == review.MenuItemId);

            if (alreadyReviewed)
            {
                return RedirectToAction("Menu", new { id = restaurantId });
            }

            var itemReview = new MenuItemReview
            {
                MenuItemId = review.MenuItemId,
                UserId = _userManager.GetUserId(User),
                Rating = review.Rating,
                CreatedAt = DateTime.Now
            };

            _context.MenuItemReviews.Add(itemReview);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Menu), new { id = restaurantId });
        }

        [Authorize(Roles = AppRoles.User + "," + AppRoles.RestaurantOwner)]
        public async Task<IActionResult> FavoriteRestaurants(int? id)
		{
			var user = await _context.Users.Include(u => u.FavoriteRestaurants)
			.ThenInclude(r => r.RestaurantCategories)
			.Include(u => u.FavoriteRestaurants)
		    .ThenInclude(r => r.Locations)
			.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

			var restaurants = user.FavoriteRestaurants.AsQueryable();

			if (id != null)
			{
				restaurants = restaurants.Where(r => r.RestaurantCategories.Any(c => c.Id == id));
			}

			var vm = new HomeViewModel
			{
				Restaurants = restaurants.ToList(),
				RestaurantsCategory = _context.RestaurantCategories.ToList(),
				FavoriteRestaurantIds = user.FavoriteRestaurants.Select(r => r.Id).ToList()
			};

			return View("Index",vm);
		}

	}
}
