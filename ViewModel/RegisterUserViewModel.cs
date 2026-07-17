using System.ComponentModel.DataAnnotations;

namespace menus_project.ViewModel
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Validation.Required")]
        [StringLength(50, ErrorMessage = "Validation.StringLength")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Validation.Required")]
        [StringLength(50, ErrorMessage = "Validation.StringLength")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Validation.Required.Email")]
        [EmailAddress(ErrorMessage = "Validation.Email")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Validation.Required")]
		[StringLength(50, ErrorMessage = "Validation.StringLength")]
		public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Validation.Required.Password")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Validation.Password.StringLength")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Validation.Required.Password")]
        [Compare("Password", ErrorMessage = "Validation.Confirm.Password")]
        [DataType(DataType.Password)]
        public string ConfirmPaasword { get; set; }

    }
}
