using menus_project.Models;

namespace menus_project.ViewModel
{
	public class RestaurantReviewViewModel
	{
		public Restaurant Restaurant { get; set; }

		public List<RestaurantReview> RestaurantReviews { get; set; }

		public RestaurantReview AddRestaurantReview {  get; set; }

		public bool IsReviewed { get; set; }

		public int RestaurantId { get; set; }

	}
}
