using Microsoft.AspNetCore.Mvc;
using Blazor_ASPMVC.Data;
using Blazor_ASPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Blazor_ASPMVC.Controllers
{
	public class PropertyController : Controller
	{
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public PropertyController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
		public async Task<IActionResult> Display()
		{
            var propertiesForSale = await _context.Properties
               .Include(p => p.PropertyImages) // Include images
               .ToListAsync();

            return View("PropertiesList", propertiesForSale);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Buy()
        {
            var userId = _userManager.GetUserId(User); // Get the ID of the logged-in user
            var propertiesForSale = await _context.Properties
                .Include(p => p.PropertyImages)
                .Where(p => p.TypeofListing == TypeofListing.Sell && p.UserID != userId && p.PropertyStatus == StatusofProperty.Listed) // Exclude the user's own properties
                .ToListAsync();

            return View("PropertiesList", propertiesForSale);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Rent()
        {
            var userId = _userManager.GetUserId(User); // Get the ID of the logged-in user
            var propertiesForRent = await _context.Properties
                .Include(p => p.PropertyImages)
                .Where(p => p.TypeofListing == TypeofListing.Rent && p.UserID != userId && p.PropertyStatus == StatusofProperty.Listed) // Exclude the user's own properties
                .ToListAsync();

            return View("PropertiesList", propertiesForRent);
        }


        [HttpGet]
		[Authorize]
		public IActionResult ListProperty()
		{

			return View(new PropertyListingViewModel());
		}

        


        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> ListProperty(PropertyListingViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
					return Unauthorized();
                }
				var property = new Property()
				{
					UserID = user.Id, // Assign the current user's ID to the property
					Price = model.Price,
					Address = model.Address,
					Beds = model.Beds,
					Baths = model.Baths,
					Area = model.Area,
					Parking = model.Parking,
					PropertyType = model.PropertyType,
					TypeofListing = model.ListingType,
                    Description = model.Description,
					PropertyStatus = StatusofProperty.Listed, // Set default status
															  // Initialize the PropertyImages collection
					PropertyImages = new List<PropertyImage>()

				};

                foreach (var image in model.PropertyImages)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", fileName);

                        // Declare the PropertyImage object outside the using block
                        var propertyImage = new PropertyImage { ImageName = fileName };

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                            // Optionally reset position to the beginning if you need to read the stream again
                            stream.Position = 0;

                            // If you need to store the byte array in the database, do it within the using block
                            using (var memoryStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memoryStream);
                                propertyImage.ImageData = memoryStream.ToArray();
                            }
                        }

                        // Add the PropertyImage object to the list after exiting the using block
                        property.PropertyImages.Add(propertyImage);
                    }
                }


                var listing = new Listing
                {
                    Property = property,
                    ListingOpenDate = DateTime.Now,
					UserID = user.Id,
					StatusofListing = 0,
                    TypeListing = (Listing.TypeofListing)property.TypeofListing
				};

                _context.Properties.Add(property);
				_context.Listings.Add(listing);
                await _context.SaveChangesAsync();

                // Redirect to a confirmation page or the property details page
                return RedirectToAction("Details", new { id = property.PropertyID });
            }

			return View(model);
		}

        public async Task<IActionResult> Details(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.PropertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            var owner = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == property.UserID);

            var viewModel = new PropertyDetailsViewModel
            {
                PropertyID = property.PropertyID,
                Price = property.Price,
                Address = property.Address,
                Beds = property.Beds,
                Baths = property.Baths,
                Area = property.Area,
                Parking = property.Parking,
                PropertyType = property.PropertyType,
                Description = property.Description,
                PropertyStatus = property.PropertyStatus,
                UserID = property.UserID,
                ImageUrls = property.PropertyImages.Select(img => "~/imgs/" + img.ImageName).ToList(), // Replace with your image URL path logic

                OwnerName = owner?.Name,
                OwnerEmail = owner?.Email,
                OwnerMobile = owner?.Mobile
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(PropertySearchViewModel searchModel)
        {
            var userId = _userManager.GetUserId(User);
            var query = _context.Properties
            .Include(p => p.PropertyImages) // Include images
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.Location))
            {
                query = query.Where(p => EF.Functions.Like(p.Address, $"%{searchModel.Location}%"));
            }

            if (searchModel.PriceMin.HasValue)
            {
                query = query.Where(p => p.Price >= searchModel.PriceMin.Value);
            }

            if (searchModel.PriceMax.HasValue)
            {
                query = query.Where(p => p.Price <= searchModel.PriceMax.Value);
            }

            if (searchModel.BedsMin.HasValue)
            {
                query = query.Where(p => p.Beds == searchModel.BedsMin.Value);
            }

            if (searchModel.BathsMin.HasValue)
            {
                query = query.Where(p => p.Baths == searchModel.BathsMin.Value);
            }

            if (searchModel.PropertyType.HasValue)
            {
                query = query.Where(p => p.PropertyType == searchModel.PropertyType.Value);
            }

            query = query.Where(p => p.UserID != userId);

            var properties = await query.ToListAsync();

            return View("PropertiesList", properties);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            // Ensure the property exists and belongs to the current user
            if (property == null || property.UserID != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var model = new PropertyDetailsViewModel
            {
                
                Price = property.Price,
                Address = property.Address,
                Beds = property.Beds,
                Baths = property.Baths,
                Area = property.Area,
                Description = property.Description,
                Parking = property.Parking,
                PropertyType = property.PropertyType,
                PropertyStatus = property.PropertyStatus
            };

            // Pass property to a view model if necessary or use directly
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(int id, PropertyDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var propertyToUpdate = await _context.Properties.FindAsync(id);
            if (propertyToUpdate == null)
            {
                return NotFound();
            }

            // Update properties
            propertyToUpdate.Address = model.Address;
            propertyToUpdate.Price = model.Price;
            propertyToUpdate.Description = model.Description;
            propertyToUpdate.Beds = model.Beds;
            propertyToUpdate.Baths = model.Baths;
            propertyToUpdate.Area = model.Area;
            propertyToUpdate.Parking = model.Parking;
            propertyToUpdate.PropertyType = model.PropertyType;
            propertyToUpdate.PropertyStatus = model.PropertyStatus;
            // Update other properties as necessary

            // Handle the property images upload, if needed

            _context.Update(propertyToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageProperties", "User");
        }






        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
    
            if (property == null || property.UserID != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProperties", "User");
        }

    }
}
