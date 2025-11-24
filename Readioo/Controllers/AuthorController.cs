using Microsoft.AspNetCore.Mvc;
using Readioo.Business.DataTransferObjects.Author;
using Readioo.Business.DataTransferObjects.Book;
using Readioo.Business.Services.Interfaces;
using Readioo.Models;
using Readioo.ViewModel;

namespace Readioo.Controllers
{
    public class AuthorController : Controller
    {

        private readonly IAuthorService _authorService;
        public AuthorController( IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public IActionResult Index()
        {
            var authors = _authorService.ShowAllAuthors();
            return View(authors);
        }

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
            return RedirectToAction(nameof(Index));
        }
    }

}
