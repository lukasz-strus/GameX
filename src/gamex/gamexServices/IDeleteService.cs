namespace gamexServices;

public interface IDeleteService
{
    Task<int> Delete(string token, int id);
}