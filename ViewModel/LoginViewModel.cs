using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace menus_project.ViewModel
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Validation.Required.Email")]
		[EmailAddress(ErrorMessage = "Validation.Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Validation.Required.Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        [ValidateNever]
        public bool RememberMe { get; set; }
        [ValidateNever]
        public string ReturnUrl { get; set; }
	}
}
