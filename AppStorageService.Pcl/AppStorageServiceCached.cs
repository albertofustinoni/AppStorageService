using AppStorageService.Core;

namespace AppStorageService.Pcl
{
    public abstract class AppStorageServiceCached<TData> : AppStorageServiceCachedBase<AppStorageService<TData>, TData> where TData : class
    {
        protected override AppStorageService<TData> CreateBackingServiceInstance(string fileName)
        {
            return new AppStorageService<TData>(fileName);
        }

        public AppStorageServiceCached(string fileName) : base(fileName) { }
    }
}
