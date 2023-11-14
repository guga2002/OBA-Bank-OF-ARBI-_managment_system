using Bank_Managment_System.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
namespace Bank_Managment_System.CookiePolicy
{
    public class CookieForManager
    {
        private readonly BankDb _database;
        private readonly IHttpContextAccessor _ict;

        public CookieForManager(BankDb db,IHttpContextAccessor _ict)
        {
            _database = db;
            this._ict = _ict;
        }
        public void ManageSessionCookieFormanager(int id)
        {

            string sessionToken = GenerateSecureToken();
            var response = _ict.HttpContext.Response;
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddHours(3);// 3 saati iqneba cookie aqtiuri
            cookieOptions.Path = "/";
            response.Cookies.Append("ManagerCookie", sessionToken, cookieOptions);
            StoreSessionToken(id, sessionToken);
        }

        private string GenerateSecureToken()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte[] randomBytes = new byte[35];

            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            StringBuilder token = new StringBuilder(35);

            foreach (byte b in randomBytes)
            {
                token.Append(allowedChars[b % allowedChars.Length]);
            }

            return token.ToString();
        }
        private void StoreSessionToken(int managerid, string sessionToken)
        {
            var res = _database.cookieformanager.Where(io => io.managerID == managerid).FirstOrDefault();
            if (res != null)
            {
                _database.cookieformanager.Remove(res);
            }
            _database.cookieformanager.Add(new Cookieformanager()
            {
                managerID = managerid,
                token = sessionToken,
            });
            _database.SaveChanges();
        }
    }
}
