using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
	public class LoginVM
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Email không đúng định dạng")]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
		ErrorMessage = "Mật khẩu phải ít nhất 8 kí tự bao gồm chữ,sô,chữ in hoa")]
		public string Password { get; set; } = null!;
	}
}
