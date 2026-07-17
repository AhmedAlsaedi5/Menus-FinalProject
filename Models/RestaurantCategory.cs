using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace menus_project.Models
{
	public class RestaurantCategory
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		[RegularExpression(@"^[a-zA-Z0-9\s\-_'.]+$", ErrorMessage = "Validation.EnglishOnly")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		[RegularExpression(@"^[\u0600-\u06FF\s\-_'.]+$", ErrorMessage = "Validation.ArabicOnly")]
		public string NameInArabic { get; set; }

		[ValidateNever]
		public ICollection<Restaurant> Restaurants { get; set; }


	}
}
