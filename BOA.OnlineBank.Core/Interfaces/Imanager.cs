using Bank_Managment_System.ResponseAndRequest;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface Imanager
    {
        Task<int> SignInManager(ManagerSignInRequest req);
        Task<int> SignUpManager(ManagerSignuprequest request);
        Task<bool> signedOutManager();
    }
}
