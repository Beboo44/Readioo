using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.User; // 2. Map to UserRegistrationDto
using Readioo.Business.Services.Interfaces; // 1. Access IUserService
using Readioo.Data.Models; // 3. For the User entity type in SignInUser
using Readioo.Models;
using Readioo.ViewModel;
// --- NEW USINGS FOR AUTHENTICATION ---
using System.Security.Claims;
using System.Threading.Tasks;
// ------------------------------------

namespace Readioo.Controllers
{
    // ✅ NO [Authorize] attribute here - users need to access this without logging in
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            bool isValid = await _userService.VerifyUserCredentialsAsync(loginVm.Email, loginVm.Password);

            if (isValid)
            {
                var user = await _userService.GetUserByEmailAsync(loginVm.Email);

                if (user != null)
                {
                    await SignInUser(user, loginVm.RememberMe);

                    return RedirectToAction("Index", "Home");

                }
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(loginVm);
        }


        // ... rest of AccountController code (Logout, Register, SignInUser) is fine ...

        // ==========================================================
        // === LOGOUT ACTION (NEW) ==================================
        // ==========================================================
        // Show confirmation page
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        // Perform actual logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        /*
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Deletes the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }*/


        // ==========================================================
        // === REGISTER ACTIONS (UPDATED TO SIGN-IN AFTER SUCCESS) ==
        // ==========================================================

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registrationDto = new UserRegistrationDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
            };

            var registrationSuccess = await _userService.RegisterUserAsync(registrationDto);

            if (registrationSuccess)
            {
                // --- NEW: Sign in user immediately after successful registration ---
                var user = await _userService.GetUserByEmailAsync(model.Email);

                if (user != null)
                {
                    await SignInUser(user, false); // isPersistent: false for registration sign-in
                    TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                    return RedirectToAction("Index", "Home");
                }
            }

            // Failure: (e.g., Email already exists)
            ModelState.AddModelError(string.Empty, "Registration failed. A user with this email already exists.");
            return View(model);
        }

        // ==========================================================
        // === AUTHENTICATION HELPER METHOD (NEW) ===================
        // ==========================================================

        private async Task SignInUser(User user, bool isPersistent)
        {
            // 1. Define Claims (The user's identity stored in the cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Use the inherited Id as the PK
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Define Authentication Properties (Persistence/Expiry)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent, // True if RememberMe was checked
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            // 3. Write the authentication cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}