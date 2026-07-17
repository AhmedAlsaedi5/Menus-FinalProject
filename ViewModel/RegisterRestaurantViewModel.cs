using menus_project.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menus_project.ViewModel
{
    public class RegisterRestaurantViewModel
    {
        public RegisterUserViewModel User { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string RestaurantName {  get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string RestaurantNameInArabic { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[Column(TypeName = "decimal(9,6)")]
		public decimal Latitude { get; set; }


		[Required(ErrorMessage = "Validation.Required")]
		[Column(TypeName = "decimal(9,6)")]
		public decimal Longitude { get; set; }

		[ValidateNever]
        public string Image_url { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Validation.Required.Image")]
        public IFormFile ImageFile { get; set; }

		[ValidateNever]
		public List<RestaurantCategory> Categories { get; set; }


		public List<int> SelectedCategoryIds { get; set; } = new();


	}
}
