using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;

#if WINDOWS_UWP
namespace AppStorageService.Universal.Test
#elif PCL_DESKTOP
namespace AppStorageService.Pcl.Desktop.Test
#elif PCL_ANDROID
namespace AppStorageService.Pcl.Desktop.Test
#else
namespace AppStorageService.Desktop.Test
#endif
{
    public class AppStorageServiceTest : AppStorageServiceTestBase<AppStorageService<TestModel>>
    {
        protected override AppStorageService<TestModel> GetServiceInstance(string storageFileName)
        {
            return new AppStorageService<TestModel>(storageFileName);
        }
    }
}
