
using Bank_Managment_System.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Bank_Managment_System.CookiePolicy
{
    public class CookieClassForOperator
    {
        private readonly BankDb _database;
        private readonly IHttpContextAccessor _ict;

        public CookieClassForOperator(BankDb db ,IHttpContextAccessor cpn)
        {
            _database = db;
            _ict = cpn;
        }
        public void ManageSessionCookieForOperator(int id)
        {
            int userId = GetOperatorId(id);

            var response = _ict.HttpContext.Response;

            string sessionToken = GenerateSecureToken();
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddHours(3);// 3 saati
            cookieOptions.Path = "/";
            response.Cookies.Append("OperatorCookie", sessionToken, cookieOptions);
            StoreSessionToken(id, sessionToken);
        }

        private int GetOperatorId(int id)
        {
            return id;
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

        private void StoreSessionToken(int userId, string sessionToken)
        {
            var res = _database.cookieforoperator.ToList();
            if (res!=null)
            {
                _database.cookieforoperator.RemoveRange(res);

            }
            _database.cookieforoperator.Add(new CookieforOperator()
            {
                OperatorID = userId,
                Token = sessionToken
            });
            _database.SaveChanges();
        }
    }
}
