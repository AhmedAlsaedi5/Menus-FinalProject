using menus_project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace menus_project.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
		{
		}

		public DbSet<Restaurant> Restaurants { get; set; }
		public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
		public DbSet<RestaurantReview> RestaurantReviews { get; set; }
		public DbSet<Location> Locations { get; set; }


		public DbSet<MenuItem> MenuItems { get; set; }
		public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
		public DbSet<MenuItemReview> MenuItemReviews { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			 builder.Entity<Restaurant>()
			.HasOne(r => r.User)
			.WithOne(u => u.Restaurant)
			.HasForeignKey<Restaurant>(r => r.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<ApplicationUser>()
		   .HasMany(u => u.FavoriteRestaurants)
		   .WithMany(r => r.FavoritedByUsers)
		   .UsingEntity(j => j.ToTable("UserFavoriteRestaurant"));

			
			builder.Entity<Restaurant>()
			.HasMany(r => r.RestaurantCategories)
			.WithMany(c => c.Restaurants)
			.UsingEntity(j => j.ToTable("RestaurantRestaurantCategory"));
		
		}


	}
}
