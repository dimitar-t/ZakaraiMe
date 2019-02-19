namespace ZakaraiMe.Web.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;

    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [StringLength(14, ErrorMessage = "Телефонният номер трябва да е дълъг между {2} и {1} символа", MinimumLength = 10)]
        [Display(Name = "Телефонен номер")]
        public string PhoneNumber { get; set; }

        public byte[] ProfilePictureBytes{ get; set; }
    }
}
