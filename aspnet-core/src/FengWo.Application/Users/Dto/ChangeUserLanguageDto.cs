using System.ComponentModel.DataAnnotations;

namespace FengWo.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}