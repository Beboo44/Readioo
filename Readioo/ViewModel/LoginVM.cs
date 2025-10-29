using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Readioo.ViewModel
{
    public class LoginVM
    {
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Pass { get; set; }

        public bool RememberMe { get; set; }
    }
}
