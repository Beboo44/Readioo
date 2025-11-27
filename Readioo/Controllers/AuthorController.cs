using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NToastNotify;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;

namespace Readioo.Controllers
{
    [Authorize]  // ✅ Protects all Book actions

    public class AuthorController : Controller
    {

        private readonly IAuthorService _authorService;
        private readonly IToastNotification _toast;
        public AuthorController( IAuthorService authorService, IToastNotification toast)
        {
            _authorService = authorService;
            _toast = toast;
        }
        public IActionResult Index()
        {
            var authors = _authorService.ShowAllAuthors();
            return View(authors);
        }


        // Create Author
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                return View(authorVM);
            }
            var exists = _authorService.getAllAuthors().
                Any(a=> a.FullName.ToLower() == authorVM.FullName.ToLower());

            if (exists)
            {
                ModelState.AddModelError("FullName", "An author with this full name already exists.");
                return View(authorVM);
            }

            AuthorCreatedDto authorDto = new AuthorCreatedDto()
            {
                FullName = authorVM.FullName,
                Bio = authorVM.Bio,
                BirthCity = authorVM.BirthCity,
                BirthCountry = authorVM.BirthCountry,
                BirthDate = authorVM.BirthDate,
                DeathDate = authorVM.DeathDate
            };
            if (authorVM.AuthorImage != null)
            {
                string SaveFolder = "images/authors/";
                SaveFolder += Guid.NewGuid().ToString() + "_" + authorVM.AuthorImage.FileName;

                string SavePath = Path.Combine("wwwroot", SaveFolder);
                authorVM.AuthorImage.CopyTo(new FileStream(SavePath, FileMode.Create));

                authorDto.AuthorImage = SaveFolder;
            }

            await _authorService.CreateAuthor(authorDto);

            _toast.AddSuccessToastMessage("Author Added Successfully");
            return RedirectToAction(nameof(Index));
        }


        // Delete Author
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var authorDto = _authorService.getAuthorById(id);
                if (authorDto == null)
                {
                    return NotFound();
                }

                // Delete the author's image file if it exists
                if (!string.IsNullOrEmpty(authorDto.AuthorImage))
                {
                    string physicalPath = Path.Combine("wwwroot", authorDto.AuthorImage.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        {
                            // Continue even if file deletion fails
                        }
                    }
                }

                // ❗ FIXED: Await the delete call
                await _authorService.DeleteAuthor(id);

                TempData["SuccessMessage"] = $"Author '{authorDto.FullName}' has been deleted successfully.";
                _toast.AddSuccessToastMessage("Author Deleted Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to delete author. Please try again.";
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }


        public IActionResult Details(int id)
        {
            // 1. Fetch the DTO (which now includes the list of Books)
            var authorDto = _authorService.getAuthorById(id);

            if (authorDto == null)
            {
                return NotFound();
            }

            // 2. Pass the DTO directly to the View
            // The View is now expecting AuthorDto, so we don't need to map to AuthorVM.
            return View(authorDto);
        }


        // Author Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var authorDto = _authorService.getAuthorById(id);
            if (authorDto == null)
            {
                return NotFound();
            }

            // Map AuthorDto -> AuthorVM for editing
            var authorVm = new AuthorVM
            {
                FullName = authorDto.FullName,
                Bio = authorDto.Bio,
                BirthCountry = authorDto.BirthCountry,
                BirthCity = authorDto.BirthCity,
                BirthDate = authorDto.BirthDate,
                DeathDate = authorDto.DeathDate
            };

            // Store the ID and image path in ViewBag
            ViewBag.AuthorId = authorDto.AuthorId;
            ViewBag.ExistingImagePath = authorDto.AuthorImage;

            return View(authorVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AuthorId = id;
                return View(authorVM);
            }

            // Get existing author to preserve image if no new upload
            var existingAuthor = _authorService.getAuthorById(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            // Create DTO for update
            var authorDto = new AuthorCreatedDto()
            {
                FullName = authorVM.FullName,
                Bio = authorVM.Bio,
                BirthCity = authorVM.BirthCity,
                BirthCountry = authorVM.BirthCountry,
                BirthDate = authorVM.BirthDate,
                DeathDate = authorVM.DeathDate,
                // Keep existing image by default
                AuthorImage = existingAuthor.AuthorImage
            };

            // Handle new image upload
            if (authorVM.AuthorImage != null)
            {
                // Delete old image file if it exists
                if (!string.IsNullOrEmpty(existingAuthor.AuthorImage))
                {
                    // Convert web path back to physical path for deletion
                    string oldPhysicalPath = Path.Combine("wwwroot", existingAuthor.AuthorImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldPhysicalPath);
                        }
                        catch
                        {
                            // Continue even if deletion fails
                        }
                    }
                }

                // Ensure directory exists
                string saveFolder = Path.Combine("wwwroot", "images", "authors");
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + "_" + authorVM.AuthorImage.FileName;
                string fullPath = Path.Combine(saveFolder, fileName);

                // Save the file
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await authorVM.AuthorImage.CopyToAsync(fileStream);
                }

                // Store the web-accessible path (NOT the physical path)
                authorDto.AuthorImage = "images/authors/" + fileName;
            }

            try
            {
                await _authorService.UpdateAuthor(id, authorDto);
                _toast.AddSuccessToastMessage("Author Updated Successfully");
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
                ViewBag.AuthorId = id;
                ViewBag.ExistingImagePath = existingAuthor.AuthorImage;
                return View(authorVM);
            }
        }
    }

}
