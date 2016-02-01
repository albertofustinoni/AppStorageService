using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public abstract class AppStorageServiceBase<TData> : IAppStorageService<TData> where TData : class
    {
        protected abstract Task SaveDataAsyncLogic(string serializedData);
        protected abstract Task<string> LoadDataAsyncLogic();
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
            var serializedData = await Task.Run(() => JsonConvert.SerializeObject(data));
            await SaveDataAsyncLogic(serializedData);
            OperationInProgress = false;
        }

        public async Task<TData> LoadDataAsync()
        {
            OperationInProgress = true;
            var serializedData = await LoadDataAsyncLogic();
            if (string.IsNullOrEmpty(serializedData))
            {
                return null;
            }

            var output = await Task.Run(() => JsonConvert.DeserializeObject<TData>(serializedData));
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
