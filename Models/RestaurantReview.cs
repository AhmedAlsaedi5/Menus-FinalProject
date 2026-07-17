using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menus_project.Models
{
	public class RestaurantReview
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Validation.Required.Rating")]
		[Range(1, 5, ErrorMessage = "Validation.Range")]
		public byte Rating { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(1000, ErrorMessage = "Validation.StringLength")]
		public string Comment { get; set; }

		[ValidateNever]
		public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;

		[Required]
		public int RestaurantId { get; set; }

		[ForeignKey("RestaurantId")]
		[ValidateNever]
		public virtual Restaurant Restaurant { get; set; }

		[Required]
		public string UserId { get; set; }


		[ForeignKey("UserId")]
		[ValidateNever]
		public virtual ApplicationUser User { get; set; }

	}
}
