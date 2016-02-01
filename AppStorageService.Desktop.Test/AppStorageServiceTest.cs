using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;

namespace AppStorageService.Desktop.Test
{
    public class AppStorageServiceTest : AppStorageServiceTestBase<AppStorageService<TestModel>>
    {
        protected override AppStorageService<TestModel> GetServiceInstance(string storageFileName)
        {
            return new AppStorageService<TestModel>(storageFileName);
        }
    }
}
