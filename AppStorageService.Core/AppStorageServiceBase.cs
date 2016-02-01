using System;
using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public abstract class AppStorageServiceBase<TData> : IAppStorageService<TData> where TData : class
    {
        protected abstract Task SaveDataAsyncLogic(TData data);
        protected abstract Task<TData> LoadDataAsyncLogic();
        protected abstract Task DeleteDataAsyncLogic();

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
