using menus_project.Constants;
using menus_project.Data;
using menus_project.Helpers;
using menus_project.Models;
using menus_project.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;

namespace menus_project.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser>  _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly Helper _helpers;

        public AccountController (SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext context, Helper helper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _helpers = helper;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel register)
		{
            if(!ModelState.IsValid)
                return View(register);

            ApplicationUser user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppRoles.User);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public IActionResult RegisterRestaurant()
		{
            RegisterRestaurantViewModel vm = new RegisterRestaurantViewModel
            {
                Categories = _context.RestaurantCategories.ToList()    
            };

			return View(vm);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterRestaurant(RegisterRestaurantViewModel register)
        {
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

			if (register.ImageFile == null)
				ModelState.AddModelError("ImageFile", "Validation.Required.Image");
			else
			{
				string extension = Path.GetExtension(register.ImageFile.FileName).ToLower();
				if (!allowedExtensions.Contains(extension))
					ModelState.AddModelError("ImageFile", "Validation.InvalidImageType");
			}

            if(!register.SelectedCategoryIds.Any())
				ModelState.AddModelError("SelectedCategoryIds", "Validation.Required.RestaurantCategorise ");

			var existingUser = await _userManager.FindByEmailAsync(register.User.Email);
			if (existingUser != null)
				ModelState.AddModelError("User.Email", "Validation.EmailExists");




				if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
				{
					UserName = register.User.Email,
					Email = register.User.Email,
					PhoneNumber = register.User.PhoneNumber,
					FirstName = register.User.FirstName,
					LastName = register.User.LastName
				};

				var result = await _userManager.CreateAsync(user, register.User.Password);


                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, AppRoles.RestaurantOwner);

					var selectedCategories = _context.RestaurantCategories.Where(c => register.SelectedCategoryIds.Contains(c.Id)).ToList();

				
					Restaurant restaurant = new Restaurant
					{
						Name = register.RestaurantName,
						NameInArabic = register.RestaurantNameInArabic,
						UserId = user.Id,
						Image_url = _helpers.SaveImage(register.ImageFile),
                        RestaurantCategories = selectedCategories
					};


					_context.Restaurants.Add(restaurant);
					_context.SaveChanges();


					Location location = new Location
                    {
                        Address = register.Address,
                        Latitude = register.Latitude,
                        Longitude = register.Longitude,
                        RestaurantId = restaurant.Id
                    };

                    _context.Locations.Add(location);
					_context.SaveChanges();

					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home");
				}
			}

			register.Categories = _context.RestaurantCategories.ToList();

			return View(register);
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = "/")
		{
			LoginViewModel loginViewModel = new LoginViewModel { ReturnUrl = returnUrl };
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (
                        login.Email,
                        login.Password,
                        login.RememberMe,   
                        lockoutOnFailure: false
                    );

                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

				ModelState.AddModelError("", "Validation.InvalidLogin");
			}

            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
