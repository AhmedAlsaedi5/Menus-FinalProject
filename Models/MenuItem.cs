using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menus_project.Models
{
	public class MenuItem
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


		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(1000, ErrorMessage = "Validation.StringLength")]

		public string Description { get; set; }
		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(1000, ErrorMessage = "Validation.StringLength")]

		public string DescriptionInArabic { get; set; }


		[Required(ErrorMessage = "Validation.Required")]
		public decimal Price { get; set; }

		[ValidateNever]
		[Column(TypeName = "decimal(3,1)")]
		public decimal AverageRating { get; set; }
		[ValidateNever]
		public int ReviewCount { get; set; }

		[ValidateNever]
		public string Image_url { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Validation.Required.Image")]
		public IFormFile? ImageFile { get; set; }

		[ValidateNever]
		public bool IsAvailable { get; set; } = true;

		[Required]
		public int RestaurantId {  get; set; }

		[ForeignKey("RestaurantId")]
		[ValidateNever]
		public virtual Restaurant Restaurant { get; set;}

		[Required]
		public int MenuItemCategoryId { get; set; }

		[ForeignKey("MenuItemCategoryId")]
		[ValidateNever]
		public virtual MenuItemCategory MenuItemCategory { get; set; }

		[ValidateNever]
		public ICollection<MenuItemReview> Reviews { get; set; }

	}
}
