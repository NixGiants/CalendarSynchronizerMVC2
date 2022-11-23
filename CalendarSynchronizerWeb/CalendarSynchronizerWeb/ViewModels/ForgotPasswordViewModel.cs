using System.ComponentModel.DataAnnotations;

namespace CalendarSynchronizerWeb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


    }
}
