using System.ComponentModel.DataAnnotations;

namespace MHPQ.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}