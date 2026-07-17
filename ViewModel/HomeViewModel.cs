using menus_project.Models;

namespace menus_project.ViewModel
{
	public class HomeViewModel
	{
		public List< Restaurant > Restaurants { get; set; }

		public List<RestaurantCategory> RestaurantsCategory { get; set; }

		public List<int> FavoriteRestaurantIds { get; set; } = new();


		private double Haversine(double lat1, double lon1, double lat2, double lon2)
		{
			const double R = 6371; // km

			double dLat = (lat2 - lat1) * Math.PI / 180;
			double dLon = (lon2 - lon1) * Math.PI / 180;

			lat1 *= Math.PI / 180;
			lat2 *= Math.PI / 180;

			double a =
				Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Cos(lat1) * Math.Cos(lat2) *
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return R * c;
		}
		public double CalculateDistance(ICollection<Location>? locations)
		{
			decimal userLatitude = 24.7136m;
			decimal userLongitude = 46.6753m;

			if (locations == null || !locations.Any())
				return -1; 

			double minDistance = double.MaxValue;

			foreach (var location in locations)
			{
				double distance = Haversine(
					(double)userLatitude,
					(double)userLongitude,
					(double)location.Latitude,
					(double)location.Longitude);

				if (distance < minDistance)
					minDistance = distance;
			}

			return minDistance;
		}
	}
}
