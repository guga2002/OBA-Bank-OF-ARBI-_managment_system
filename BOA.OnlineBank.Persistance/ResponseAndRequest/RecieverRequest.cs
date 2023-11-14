using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class RecieverRequest
    { 
        public string Iban{ get; set; }
        public ValuteEnum valut { get; set; }
       
    }
}
