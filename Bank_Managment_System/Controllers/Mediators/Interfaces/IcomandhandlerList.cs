namespace Bank_Managment_System.Controllers.Mediators.Interfaces
{
    public interface IcomandhandlerList<T, J>
    {
        Task<List<T>> Handle(J command);
    }
}
