namespace ZakaraiMe.Web.Models.Users
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class UserFormViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [EmailAddress]
        [Display(Name = "Ел. поща")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [StringLength(50, ErrorMessage = "Името трябва да e дългo между {2} и {1} символа.", MinimumLength = 3)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [StringLength(50, ErrorMessage = "Фамилята трябва да е дългa между {2} и {1} символа.", MinimumLength = 3)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [StringLength(100, ErrorMessage = "Паролата трябва да е дълга между {2} и {1} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Двете пароли не съвпадат.")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [StringLength(14, ErrorMessage = "Телефонният номер трябва да е дълъг между {2} и {1} символа", MinimumLength = 10)]
        [Display(Name = "Телефонен номер")]
        public string PhoneNumber { get; set; }

        public string FileName { get; set; }
        
        [Display(Name = "Профилна снимка")]
        public IFormFile ImageFile { get; set; }
    }
}
