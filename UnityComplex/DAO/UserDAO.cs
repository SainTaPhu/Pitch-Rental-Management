using UnityComplex.Models;

namespace UnityComplex.DAO
{
	public class UserDAO
	{

		private readonly UnityteamContext db;

		public UserDAO() { }
		public UserDAO(UnityteamContext context)
		{
			db = context;
		}


        public User SelectUser(int? idUser)
        {
            try
            {
                var result = db.Users.Where(i => i.UserId == idUser).FirstOrDefault();
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public int SelectRoleUser(int? idUser)
        {
            try
            {

                int result = db.Users.Where(id => id.UserId == idUser).Select(i => i.IdRole).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return -1;
        }


    }
}

