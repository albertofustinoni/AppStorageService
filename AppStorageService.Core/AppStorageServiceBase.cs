using System;
using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public abstract class AppStorageServiceBase<TData> : IAppStorageService<TData> where TData : class
    {
        public abstract Task SaveDataAsyncLogic(TData data);
        public abstract Task<TData> LoadDataAsyncLogic();
        public abstract Task DeleteDataAsyncLogic();

        public readonly string FileName;
        public bool OperationInProgress { get; private set; }

        public AppStorageServiceBase(string fileName)
        {
            FileName = fileName;
            OperationInProgress = false;
        }

        public async Task SaveDataAsync(TData data)
        {
            OperationInProgress = true;
            await SaveDataAsyncLogic(data);
            OperationInProgress = false;
        }

        public async Task<TData> LoadDataAsync()
        {
            OperationInProgress = true;
            var output = await LoadDataAsyncLogic();
            OperationInProgress = false;
            return output;
        }

        public async Task DeleteDataAsync()
        {
            OperationInProgress = true;
            await DeleteDataAsyncLogic();
            OperationInProgress = false;
        }
    }
}
