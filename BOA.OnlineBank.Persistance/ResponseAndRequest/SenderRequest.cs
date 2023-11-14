using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class SenderRequest
    {
        public string Iban { get; set; }
        public ValuteEnum valut { get; set; }
        public decimal Amount { get; set; }
    }
}
