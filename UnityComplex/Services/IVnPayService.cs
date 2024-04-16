using UnityComplex.ViewModels;

namespace UnityComplex.Services
{
    public interface IVnPayService
    {
        string CreateRequestUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
