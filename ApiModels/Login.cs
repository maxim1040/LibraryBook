using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBook.ApiModels
{
    public class LoginModel
    {
        [Key]
        public int ? Id { get; set; }   
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
