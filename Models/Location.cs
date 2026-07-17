using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menus_project.Models
{
	public class Location
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[Column(TypeName = "decimal(9,6)")]
		public decimal Latitude { get; set; }


		[Required(ErrorMessage = "Validation.Required")]
		[Column(TypeName = "decimal(9,6)")]
		public decimal Longitude { get; set; }

		[Required]
		public int RestaurantId { get; set; }

		[ForeignKey("RestaurantId")]
		[ValidateNever]
		public virtual Restaurant Restaurant { get; set; }
	}
}
