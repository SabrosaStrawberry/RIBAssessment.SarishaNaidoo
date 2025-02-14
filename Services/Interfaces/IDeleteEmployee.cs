namespace Services.Interfaces
{
    public interface IDeleteEmployee
    {
        Task<bool> DeleteEmployeeAsync(int Id);
    }
}
