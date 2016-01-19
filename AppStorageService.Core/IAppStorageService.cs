using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public interface IAppStorageService<TData> where TData : class
    {
        bool OperationInProgress { get; }
        Task SaveDataAsync(TData data);
        Task<TData> LoadDataAsync();
    }
}
