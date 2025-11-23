using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.User;
using Readioo.Business.DTO;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;
using System.ComponentModel.Design;
using System.Security.Claims;


namespace Readioo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        // Inject IUserService 
        public UserController(IUserService userService)
        {
            _userService = userService;
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
                ProfileUrl = user.ProfileUrl,
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

        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            var vm = new UpdateUserVM
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                City = user.City,
                Country = user.Country,
                ProfileUrl = user.ProfileUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            UpdateUserDTO dto = new()
            {
                UserId = vm.UserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Bio = vm.Bio,
                City = vm.City,
                Country = vm.Country,
                ProfileUrl = vm.ProfileUrl
            };

            if (vm.UserImage != null)
            {
                using var ms = new MemoryStream();
                await vm.UserImage.CopyToAsync(ms);
                dto.UserImage = ms.ToArray();
            }

            var result = await _userService.UpdateUserProfileAsync(vm.UserId, dto);

            if (result)
                return RedirectToAction("Profile");

            ModelState.AddModelError("", "Update Failed");
            return View(vm);
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




    }
}
