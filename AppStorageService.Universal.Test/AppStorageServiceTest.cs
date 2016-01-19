using AppStorageService.Core;
using AppStorageService.Core.Test;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppStorageService.Universal.Test
{
    public class AppStorageServiceTest : AppStorageServiceTestBase<AppStorageService<TestModel>>
    {
        protected override async Task DeleteStorageFile(string storageFileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(storageFileName);
            await file.DeleteAsync();
        }

        protected override IAppStorageService<T> GetServiceInstance<T>(string storageFileName)
        {
            return new AppStorageService<T>(storageFileName);
        }
    }
}
