using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Refund
    {
        public int IdRefund { get; set; }
        public int? UserId { get; set; }
        public string? UserVnpay { get; set; }
        public int? IdPayment { get; set; }
        public string? BankName { get; set; }
        public string? NameOfBeni { get; set; }
        public string? AccNumber { get; set; }
        public int? NumberMoney { get; set; }

        public virtual Payment? IdPaymentNavigation { get; set; }
    }
}
