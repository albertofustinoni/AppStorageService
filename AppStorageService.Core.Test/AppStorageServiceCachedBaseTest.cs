using AppStorageService.Core.Test.Mocks;
using AppStorageService.Core.Test.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AppStorageService.Core.Test
{
    public class AppStorageServiceCachedBaseTest
    {
        class TestService : AppStorageServiceCachedBase<AppStorageServiceMock, TestModel>
        {
            public event Func<TestModel, TestModel> CloneDataHandler;
            public AppStorageServiceMock BackingStore { get; private set; }

            public TestService() : base(null) { }

            protected override TestModel CloneData(TestModel input) { return CloneDataHandler(input); }

            protected override AppStorageServiceMock CreateBackingServiceInstance(string fileName)
            {
                BackingStore = new AppStorageServiceMock();
                return BackingStore;
            }
        }

        [Fact]
        public async Task SaveDataAsyncWorks()
        {
            var data = new TestModel();
            var service = new TestService();

            var saveDataCalled = false;
            service.BackingStore.SaveDataAsyncHandler += d =>
            {
                saveDataCalled = true;
                Assert.Same(data, d);
                return Task.FromResult(true);
            };

            await service.SaveDataAsync(data);
            Assert.True(saveDataCalled);
        }

        [Fact]
        public Task LoadDataAsyncWorks()
        {
            return DataLoadingTestBase(false);
        }

        [Fact]
        public Task DeleteDataAsyncWorks()
        {
            return DataLoadingTestBase(true);
        }

        private async Task DataLoadingTestBase(bool deleteAfterLoad)
        {
            const int numItarations = 4;

            var loadedData = new TestModel();
            var clonedData = new TestModel();

            var service = new TestService();

            var numLoadDataCalled = 0;
            service.BackingStore.LoadDataAsyncHandler += () =>
            {
                numLoadDataCalled++;
                return Task.FromResult(loadedData);
            };

            var numDeleteDataCalled = 0;
            service.BackingStore.DeleteDataAsyncHandler += () =>
            {
                numDeleteDataCalled++;
                return Task.FromResult(true);
            };

            var numCloneDataCalled = 0;
            service.CloneDataHandler += d =>
            {
                numCloneDataCalled++;
                Assert.Same(loadedData, d);
                return clonedData;
            };

            for (var i = 0; i < numItarations; i++)
            {
                var loadResult = await service.LoadDataAsync();
                if(deleteAfterLoad)
                {
                    await service.DeleteDataAsync();
                }
                Assert.Same(clonedData, loadResult);
            }

            var numExpectedLoads = deleteAfterLoad ? numItarations : 1;
            Assert.Equal(numExpectedLoads, numLoadDataCalled);
            var numExpectedDeletes = deleteAfterLoad ? numItarations : 0;
            Assert.Equal(numExpectedDeletes, numDeleteDataCalled);
            Assert.Equal(numItarations, numCloneDataCalled);
        }
    }
}
