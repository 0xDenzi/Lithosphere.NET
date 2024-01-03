using Blazor_ASPMVC.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blazor_ASPMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Blazor_ASPMVC.Controllers
{
    
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _db;

        public UserController(AppDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, Mobile = model.Mobile, Name = model.Name, Address = model.Address, AccountCreationDate = DateTime.UtcNow };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var model = new EditProfileViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Mobile = user.Mobile,
                Address = user.Address,
                Description = user.Description,
                CreationDateTime = user.AccountCreationDate,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if(user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Mobile = model.Mobile;
                user.Email = model.Email;
                user.Address = model.Address;
                user.Description = model.Description;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Begin a transaction to ensure all deletions are successful
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    // Remove user's properties
                    var userProperties = _db.Properties.Where(p => p.UserID == user.Id);
                    _db.Properties.RemoveRange(userProperties);

                    // Remove user's bookmarks
                    var userBookmarks = _db.Bookmarks.Where(b => b.UserID == user.Id);
                    _db.Bookmarks.RemoveRange(userBookmarks);

                    // Remove other users' bookmarks for the user's properties
                    var propertiesIds = userProperties.Select(p => p.PropertyID).ToList();
                    var bookmarksOfUserProperties = _db.Bookmarks.Where(b => propertiesIds.Contains(b.PropertyID));
                    _db.Bookmarks.RemoveRange(bookmarksOfUserProperties);

                    // Save changes to the database
                    await _db.SaveChangesAsync();

                    // Delete the user account
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        // If the user could not be deleted, throw an exception to roll back the transaction
                        throw new InvalidOperationException("Could not delete user account.");
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();

                    // Sign out the user
                    await _signInManager.SignOutAsync();

                    // Redirect to the home page or a confirmation page
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    // If there was an error, roll back the transaction
                    await transaction.RollbackAsync();
                    // Log the error or display an error message to the user
                    // ...
                }
            }

            // If we get here, something went wrong
            return View("Error");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleBookmark(int propertyId)
        {
            var userId = _userManager.GetUserId(User);
            var bookmark = await _db.Bookmarks.FirstOrDefaultAsync(
                b => b.PropertyID == propertyId && b.UserID == userId);

            bool isNowBookmarked;
            if (bookmark == null)
            {
                // Bookmark doesn't exist, create it
                var newBookmark = new Bookmark { PropertyID = propertyId, UserID = userId, BookmarkDate = DateTime.Now };
                _db.Bookmarks.Add(newBookmark);
                isNowBookmarked = true;
            }
            else
            {
                // Bookmark exists, remove it
                _db.Bookmarks.Remove(bookmark);
                isNowBookmarked = false;
            }

            await _db.SaveChangesAsync();

            return Json(new { isBookmarked = isNowBookmarked });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListBookmarks()
        {
            var userId = _userManager.GetUserId(User);
            var bookmarkedProperties = await _db.Bookmarks
                .Include(b => b.Property)  // Include the Property
                .ThenInclude(p => p.PropertyImages)  // Then include the PropertyImages
                .Where(b => b.UserID == userId)
                .Select(b => b.Property)  // Now select the Property
                .Distinct()  // If you want to avoid duplicates
                .ToListAsync();

            // Pass the bookmarked properties to the view
            return View("ListBookmarks", bookmarkedProperties);
        }

        [Authorize]
        public async Task<IActionResult> ManageProperties()
        {
            var userId = _userManager.GetUserId(User); // Make sure to inject UserManager<User> in your controller
            var userProperties = await _db.Properties
                                               .Where(p => p.UserID == userId)
                                               .ToListAsync(); // Fetch properties where the current user is the owner

            return View(userProperties); // Return the view with the list of properties
        }
    }
}