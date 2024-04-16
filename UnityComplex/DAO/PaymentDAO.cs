using UnityComplex.Models;

namespace UnityComplex.DAO
{
    public class PaymentDAO
    {
        private readonly UnityteamContext db;

        public PaymentDAO() { }
        public PaymentDAO(UnityteamContext context)
        {
            db = context;
        }

        public List<Payment> SelectAllPaymentOfUser(int check, int idUser)
        {
            try
            {
                if (check == 0)
                {
                    var result = db.Payments.Where(i => i.IdUser == idUser).ToList();
                    return result;
                }
                else
                {
                    var result = db.Payments.Where(i => i.IdUser == idUser).GroupBy(p => p.IdBooking).Select(g => g.FirstOrDefault()).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public void Add(Payment payment)
        {
            try
            {
                db.Add(payment);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Payment selectPayment(int? idP)
        {

            try
            {
                var result2 = db.Payments.Find(idP);

                return result2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
        public void AddRefund(Refund refund)
        {
            try
            {
                db.Add(refund);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
