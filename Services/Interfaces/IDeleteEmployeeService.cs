namespace Services.Interfaces
{
    public interface IDeleteEmployeeService
    {
        Task<bool> DeleteEmployeeAsync(int Id);
    }
}
