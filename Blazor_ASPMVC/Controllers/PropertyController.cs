using Microsoft.AspNetCore.Mvc;
using Blazor_ASPMVC.Data;
using Blazor_ASPMVC.Models;

namespace Blazor_ASPMVC.Controllers
{
	public class PropertyController : Controller
	{
		private readonly AppDbContext _db;

        public PropertyController(AppDbContext db)
        {
			_db = db;
        }

        [HttpGet]
		public IActionResult Display()
		{
			IEnumerable<Property> propertyList = _db.Properties;
			return View(propertyList);
		}
	}
}
