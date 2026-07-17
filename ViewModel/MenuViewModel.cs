using menus_project.Models;

namespace menus_project.ViewModel
{
	public class MenuViewModel
	{
		public Restaurant Restaurant { get; set; }

		public bool IsFavoriteRestaurant { get; set; }

		public List<MenuItem> MenuItems { get; set; }

		public List<MenuItemCategory> MenuItemCategories { get; set; }

		public MenuItemReview Review { get; set; }

		public bool IsReviewed { get; set; }
    }
}
