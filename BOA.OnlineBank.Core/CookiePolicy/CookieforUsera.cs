
using Microsoft.AspNetCore.Http;
using System.Text;
using Bank_Managment_System.Models;

namespace Bank_Managment_System.CookiePolicy
{
    public class CookieforUsera
    {
        private readonly BankDb _database;
        private readonly IHttpContextAccessor _ict;

        public CookieforUsera(BankDb db, IHttpContextAccessor ict)
        {
            _database = db;
            _ict = ict;
        }
        public void ManageSessionCookieForUser(int id)
        {
            string sessionToken = GenerateSecureToken();
            var response = _ict.HttpContext.Response;
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddHours(3);// sami saati
            cookieOptions.Path = "/";
            response.Cookies.Append("UserCookies", sessionToken, cookieOptions);
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
        private void StoreSessionToken(int userid, string sessionToken)
        {
            var res = _database.cookieforuser.Where(io => io.UserID == userid).FirstOrDefault();
            if (res != null)
            {
                _database.cookieforuser.Remove(res);
            }
            _database.cookieforuser.Add(new CookieforUser()
            {
                UserID = userid,
                token = sessionToken
            });
            _database.SaveChanges();
        }
    }
}
