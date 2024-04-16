using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
	public class RegisterVM
	{
		[Required(ErrorMessage = "Yêu cầu nhập email")]
		[EmailAddress(ErrorMessage = "Email không đúng định dạng (abc@gmail.com)")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
		ErrorMessage = "Mật khẩu phải ít nhất 8 kí tự bao gồm chữ,số,chữ in hoa")]
		public string Password { get; set; } 
		[Required(ErrorMessage = "Yêu cầu nhập SĐT")]
		[Phone(ErrorMessage = "Không đúng định dạng SĐT")]

        [RegularExpression(@"^(0|9)\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng số 0 hoặc 9 và chứa đúng 10 chữ số")]
        public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên")]
		public string? FullName { get; set; }



		[Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
		public string? Address { get; set; }
	}
}
