using AppStorageService.Core;
using AppStorageService.Core.Test;

namespace AppStorageService.Desktop.Test
{
    public class AppStorageServiceTest : AppStorageServiceTestBase<AppStorageService<TestModel>>
    {
        protected override IAppStorageService<T> GetServiceInstance<T>(string storageFileName)
        {
            return new AppStorageService<T>(storageFileName);
        }
    }
}
