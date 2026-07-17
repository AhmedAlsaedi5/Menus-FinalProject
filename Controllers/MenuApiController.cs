using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using menus_project.Data;

namespace menus_project.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{restaurantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMenu(int restaurantId)
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemCategory)
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.NameInArabic,
                    m.Description,
                    m.DescriptionInArabic,
                    m.Price,
                    m.Image_url,
                    m.AverageRating,
                    m.ReviewCount,
                    Category = m.MenuItemCategory.Name,
                    CategoryArabic = m.MenuItemCategory.NameInArabic
                })
                .ToListAsync();

            if (!menuItems.Any())
                return NotFound("لا توجد أصناف لهذا المطعم.");

            return Ok(menuItems);
        }
    }
}