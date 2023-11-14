namespace Bank_Managment_System.ResponseAndRequest
{
    public class TransactionStatsResponse
    {
        public int CountOfTransfer { get; set; }

        public decimal TotalwithdrawedGel { get; set; }

        public decimal TotalwithdrawedUSD { get; set; }

        public decimal TotalwithdrawedEuro { get; set; }

        public decimal TotalIncomeGel { get; set; }

        public decimal TotalIncomeUSD { get; set; }

        public decimal TotalIncomeEURo { get; set; }

        // public Dictionary<int,int> chart  { get; set; }

    }
}
