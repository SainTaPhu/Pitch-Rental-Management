using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
    public class ChangePassVM
    {
        public int idUser { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất 8 ký tự, trong đó có ít nhất một chữ cái viết hoa, một chữ cái viết thường.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
