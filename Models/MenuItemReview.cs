using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace menus_project.Models
{
	public class MenuItemReview
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Validation.Required.Rating")]
		[Range(1, 5, ErrorMessage = "Validation.Range")]
		public byte Rating { get; set; }

		[ValidateNever]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		[Required]
		public int MenuItemId { get; set; }

		[ForeignKey("MenuItemId")]
		[ValidateNever]
		public virtual MenuItem MenuItem { get; set; }


		[Required]
		public string UserId { get; set; }


		[ForeignKey("UserId")]
		[ValidateNever]
		public virtual ApplicationUser User { get; set; }

	}
}
