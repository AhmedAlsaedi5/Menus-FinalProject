using menus_project.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace menus_project.ViewModel
{
	public class MenuItemViewModel
	{
		[ValidateNever]
		public MenuItem Item { get; set; }
		[ValidateNever]
		public List<MenuItem> Items { get; set; }
		[ValidateNever]
		public List<MenuItemCategory> Categories { get; set; }


	}
}
