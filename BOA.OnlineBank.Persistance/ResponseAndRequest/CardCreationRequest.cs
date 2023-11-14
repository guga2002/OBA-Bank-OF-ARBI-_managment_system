using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class CardCreationRequest
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public ValuteEnum Valute { get; set; }

    }
}
