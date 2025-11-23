namespace Readioo.ViewModel
{
    public class UpdateUserVM
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfileUrl { get; set; }

        public IFormFile? UserImage { get; set; }
    }
}
