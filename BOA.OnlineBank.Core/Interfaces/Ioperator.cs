using Bank_Managment_System.ResponseAndRequest;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface Ioperator
    {
        Task<int> signUpOperator(OperatorRequest request);
        Task<int> SignInOperator(OperatorSignInRequest req);
        Task<bool> TwostepAuntification(string codeprovidedByUser);
        Task<bool> InicializeValute();
        Task<bool> SignOutOperator();
    }
}
