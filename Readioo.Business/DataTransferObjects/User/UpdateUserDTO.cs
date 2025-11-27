using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Business.DataTransferObjects.User
{
    public class UpdateUserDTO
    {

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public byte[]? UserImage { get; set; }  // optional
    }
}
