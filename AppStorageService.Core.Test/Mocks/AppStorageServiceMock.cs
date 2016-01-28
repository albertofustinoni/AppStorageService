using AppStorageService.Core.Test.Models;
using System;
using System.Threading.Tasks;

namespace AppStorageService.Core.Test.Mocks
{
    class AppStorageServiceMock : IAppStorageService<TestData>
    {
        public event Func<Task> DeleteDataAsyncHandler;
        public event Func<Task<TestData>> LoadDataAsyncHandler;
        public event Func<TestData, Task> SaveDataAsyncHandler;

        public bool OperationInProgress { get { return false; } }

        public Task DeleteDataAsync()
        {
            return DeleteDataAsyncHandler();
        }

        public Task<TestData> LoadDataAsync()
        {
            return LoadDataAsyncHandler();
        }

        public Task SaveDataAsync(TestData data)
        {
            return SaveDataAsyncHandler(data);
        }
    }
}
