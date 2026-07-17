using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menus_project.Models
{
	public class Restaurant
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string NameInArabic { get; set; }

		[ValidateNever]
		public string Image_url {  get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Validation.Required.Image")]
		public IFormFile ImageFile { get; set; }

		[ValidateNever]
		[Column(TypeName = "decimal(3,1)")]
		public decimal AverageRating { get; set; }
		[ValidateNever]
		public int ReviewCount {  get; set; }

		[ValidateNever]
		public bool IsActive { get; set; } = true;

		[Required]
		public string UserId { get; set; }


		[ForeignKey("UserId")]
		[ValidateNever]
		public virtual ApplicationUser User { get; set; }

		[ValidateNever]
		public ICollection<ApplicationUser> FavoritedByUsers { get; set; }


		[ValidateNever]
		public ICollection<RestaurantCategory> RestaurantCategories { get; set; }

		[ValidateNever]
		public ICollection<MenuItem> MenuItems { get; set; }
		[ValidateNever]
		public ICollection<RestaurantReview> Reviews { get; set; }

		[ValidateNever]
		public ICollection<Location> Locations { get; set; }



	}
}
