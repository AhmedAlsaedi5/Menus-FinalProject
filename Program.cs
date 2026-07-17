using menus_project.Data;
using menus_project.Helpers;
using menus_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace menus_project
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); 

			builder.Services.AddControllersWithViews()
			    .AddViewLocalization()
			    .AddDataAnnotationsLocalization(options =>
			    {
			        options.DataAnnotationLocalizerProvider = (type, factory) =>
			            factory.Create(typeof(SharedResource));
			    });


			 var supportedCultures = new[] { "ar", "en-US" };

			 builder.Services.Configure<RequestLocalizationOptions>(options =>
			 {
			     options.SetDefaultCulture("ar")
			            .AddSupportedCultures(supportedCultures)
			            .AddSupportedUICultures(supportedCultures);
             });


			// Identity
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			builder.Services.AddScoped<Helper>();


            //Api
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new ()
                {
                    Title = "Menus API",
                    Version = "v1",
                    Description = "API لجلب قوائم المطاعم والتقييمات"
                });
            });

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// SeedData

			using (var scope = app.Services.CreateScope())
			{
               
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               

                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                await SeedData.SeedRolesAndAdmin(userManager, roleManager);
                SeedData.SeedCategories(context);
			    await SeedData.SeedSampleDataAsync(context, userManager, environment);
                
            }


            app.UseHttpsRedirection();
			app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();


            // app.UseSession();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Menus API v1");
                    options.RoutePrefix = "swagger"; // الرابط: /swagger
                });
            }

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
