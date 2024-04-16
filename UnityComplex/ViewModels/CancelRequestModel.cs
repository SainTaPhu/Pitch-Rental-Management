using System.ComponentModel.DataAnnotations;

namespace UnityComplex.ViewModels
{
	public class CancelRequestModel
	{
		public int orderId { get; set; }
		public int refundAmount { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Account number must contain only numbers.")]
        public string? accountNumber { get; set; }
		public string? cancellationReason { get; set; }

        public string? BankName { get; set; }
        public string? NameAcc { get; set; }

        public int? idUser { get; set; }
        public int idPayment { get; set; }

	}
}
