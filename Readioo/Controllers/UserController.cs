using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.User;
using Readioo.Business.DTO;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.ComponentModel.Design;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;


namespace Readioo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IBookService? bookService;

        // Inject IUserService 
        public UserController(IUserService userService)
        {
            _userService = userService;
            _bookService = bookService;
        }

        public IActionResult Show()
        {
            return View();
        }
       
        
    public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));
            var userIdValue = int.Parse(userId);
            var shelfDtos = await _userService.GetUserShelvesAsync(userIdValue);


            var vm = new UserProfileVM
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Bio = user.Bio,
                City = user.City,
                Country = user.Country,
                UserImage = user.UserImage,
                Shelves = shelfDtos.Select(s => new ShelfInfoVM
                {
                    ShelfId = s.ShelfId,
                    ShelfName = s.ShelfName,
                    BooksCount = s.BooksCount
                }).ToList()
            };
        

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            var vm = new UpdateUserVM
            {
                //Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                City = user.City,
                Country = user.Country,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserVM vm)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int authenticatedUserId = int.Parse(userIdClaim);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                byte[]? imageBytes = null;

                if (vm.UserImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await vm.UserImageFile.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }

                UpdateUserDTO dto = new()
                {
                    UserId = authenticatedUserId,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Bio = vm.Bio,
                    City = vm.City,
                    Country = vm.Country,
                    UserImage = imageBytes
                };

                var result = await _userService.UpdateUserProfileAsync(dto.UserId, dto);

                if (result)
                    return RedirectToAction("Profile");

                ModelState.AddModelError("", "Update Failed");
                return View(vm);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(vm);
            }
        }
        public async Task<IActionResult> ProfileImage(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null || user.UserImage == null || user.UserImage.Length == 0)
            {
                // Return default profile image
                return PhysicalFile(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/default-profile.png"),
                    "image/png"
                );
            }

            return File(user.UserImage, "image/jpeg");
        }

        [HttpGet]
    
        public async Task<IActionResult> MyBooks()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _userService.GetUserByIdAsync(userId);

            var shelves = await _userService.GetUserShelvesWithBooksAsync(userId);

            var books = await _bookService.GetUserBooksAsync(userId);

            var vm = new MyBooksViewModel
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Shelveswithbook = shelves
 
            };

            return View(vm);
        }





    }
}
