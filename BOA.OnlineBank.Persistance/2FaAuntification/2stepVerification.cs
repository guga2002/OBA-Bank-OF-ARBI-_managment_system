using System.Net.Mail;
using System.Net;

namespace Bank_Managment_System._2FaAuntification
{
    public class _2stepVerification
    {
        public string server { get; set; } // server name   that response  for  SMTP
        public string username { get; set; } 
        public string password { get; set; }
        public int Port { get; set; }// porti romelzec gawerilia  sMtp protokoli
        public string from { get; set; }
        public _2stepVerification()
        {
            
        }
        public _2stepVerification(string server, int port,string username, string password, string from)
        {
            this.server = server;this.Port = port;this.username=username;this.password = password;this.from = from;
        }
        public string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public void SendEmailCode(string toEmailAddress, string code)
        {

            using (var client = new SmtpClient(server, Port))
            {
                client.Credentials = new NetworkCredential(username, password);//  cloudtan dakavshireba
                client.EnableSsl = true;

                using (var mailMessage = new MailMessage(from, toEmailAddress))
                {
                    mailMessage.Subject = "verifikaciis kodi";
                    mailMessage.Body = $"sheni verifikaciis kodi aris: {code}";
                    client.Send(mailMessage);
                }
            }
        }
    }
}
