namespace Bank_Managment_System.ResponseAndRequest
{
    public class TransferToOwnAccountRequest
    {
        public SenderRequest sender { get; set; }
        public RecieverRequest reciever { get; set; }
    }
}
