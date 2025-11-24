using Readioo.ViewModel;
namespace Readioo.ViewModel
{
    public class UserProfileVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfileUrl { get; set; }
        public byte[]? UserImage { get; set; }
        public List<ShelfInfoVM> Shelves { get; set; } = new ();


    }
}
