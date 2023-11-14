

using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Bank_Managment_System.MesageSenderUsingTwilo
{
    public class SendSmsToUser
    {
        private readonly IConfiguration _configuration;
        public SendSmsToUser(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool sentMesageTouser(string text, string Phonenumber)
        {
            try
            {
                string sid = _configuration["TwilioConfig:AccountSid"];
                string token = _configuration["TwilioConfig:AuthToken"];
                string phonenumber = _configuration["TwilioConfig:PhoneNumber"];//gamgzavnis  telefoni
                TwilioClient.Init(sid, token);

                var message = MessageResource.Create(
                    body: text,
                    from: new PhoneNumber(phonenumber),
                    to: new PhoneNumber(Phonenumber)
                );
                if (message.Status == MessageResource.StatusEnum.Sent)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;//ar minda rom avariulad dasruldes programa
            }
        }

    }
}
