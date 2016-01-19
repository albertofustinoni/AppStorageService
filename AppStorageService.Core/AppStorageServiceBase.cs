using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public abstract class AppStorageServiceBase<TData> : IAppStorageService<TData> where TData : class
    {
        public readonly string FileName;
        public bool OperationInProgress { get; protected set; }

        public AppStorageServiceBase(string fileName)
        {
            FileName = fileName;
            OperationInProgress = false;
        }

        public abstract Task SaveDataAsync(TData data);

        public abstract Task<TData> LoadDataAsync();
    }
}
