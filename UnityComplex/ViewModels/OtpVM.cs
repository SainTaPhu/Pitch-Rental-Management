using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
	public class OtpVM
	{
		[Required(ErrorMessage = "Không được để trống")]
		
		public string OTP { get; set; } = null!;
	}
}
