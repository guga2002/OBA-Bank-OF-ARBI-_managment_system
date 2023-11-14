using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class tRansresponse
    {
        public decimal Amount { get; set; }

        public ValuteEnum Valute { get; set; }

        public string? Sender_Iban { get; set; }

        public string? Reciever_Iban { get; set; }

        public string Transaction_Type { get; set; }

        public decimal? TotalIncome { get; set; }
        public DateTime TransactTime { get; set; }
    }
}
