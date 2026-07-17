using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace menus_project.Models
{
	public class ApplicationUser : IdentityUser
	{

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string LastName { get; set; }


		[ValidateNever]
		public Restaurant? Restaurant { get; set; }

		[ValidateNever]
		public ICollection<Restaurant> FavoriteRestaurants { get; set; }

		[ValidateNever]
		public ICollection<RestaurantReview> Reviews { get; set; }

		[ValidateNever]
		public ICollection<MenuItemReview> MenuItemReviews { get; set; }


	}
}
