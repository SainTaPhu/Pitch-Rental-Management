using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
    public class ForgotPassVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng(abc@gmail.com)")]
        public string Email { get; set; } = null!;
    }
}
