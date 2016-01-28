using System.Threading.Tasks;

namespace AppStorageService.Core
{
    public abstract class CachedAppStorageServiceBase<TService, TData> : IAppStorageService<TData> where TService : IAppStorageService<TData>, new() where TData : class
    {
        public abstract TService CreateBackingServiceInstance(string fileName);
        public abstract TData CloneData(TData input);

        protected TService BackingService { get; set; }
        protected TData CachedData { get; set; }

        public bool OperationInProgress { get { return BackingService.OperationInProgress; } }

        public CachedAppStorageServiceBase(string fileName)
        {
            CachedData = null;
            BackingService = CreateBackingServiceInstance(fileName);
        }

        public Task SaveDataAsync(TData data)
        {
            CachedData = data;
            return BackingService.SaveDataAsync(CachedData);
        }

        public async Task<TData> LoadDataAsync()
        {
            if (CachedData == null)
            {
                CachedData = await BackingService.LoadDataAsync();
            }

            var output = CloneData(CachedData);
            return output;
        }

        public Task DeleteDataAsync()
        {
            CachedData = null;
            return BackingService.DeleteDataAsync();
        }
    }
}
