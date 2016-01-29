using AppStorageService.Core.Test.Models;
using System;
using System.Threading.Tasks;

namespace AppStorageService.Core.Test.Mocks
{
    class AppStorageServiceMock : IAppStorageService<TestModel>
    {
        public event Func<Task> DeleteDataAsyncHandler;
        public event Func<Task<TestModel>> LoadDataAsyncHandler;
        public event Func<TestModel, Task> SaveDataAsyncHandler;

        public bool OperationInProgress { get { return false; } }

        public Task DeleteDataAsync()
        {
            return DeleteDataAsyncHandler();
        }

        public Task<TestModel> LoadDataAsync()
        {
            return LoadDataAsyncHandler();
        }

        public Task SaveDataAsync(TestModel data)
        {
            return SaveDataAsyncHandler(data);
        }
    }
}
