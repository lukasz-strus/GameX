using gamexModels;

namespace gamexServices;

public interface IGetService<T>
    where T : IDto
{
    Task<GetAllResult<T>> GetAll(string token, GetAllQuery query);

    Task<T> Get(string token, int id);
}