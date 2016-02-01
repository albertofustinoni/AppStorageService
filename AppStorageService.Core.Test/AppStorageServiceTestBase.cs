using AppStorageService.Core.Test.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AppStorageService.Core.Test
{
    [Collection("Collection1")]
    public abstract class AppStorageServiceTestBase<TService, TData> where TService: IAppStorageService<TData> where TData : class
    {
        private const string StorageFileName = "FileName.dat";

        protected abstract TService GetServiceInstance(string storageFileName);
        protected abstract TData GenerateTestData();
        protected abstract bool CompareEquality(TData reference, TData value);

        [Fact]
        public async Task LoadFromStorageAsync_Returns_Null_If_No_Data_Is_Present()
        {
            var service = GetServiceInstance(StorageFileName);
            await service.DeleteDataAsync();

            var data = await service.LoadDataAsync();
            Assert.Null(data);

            await CleanUp();
        }

        [Fact]
        public async Task LoadFromStorageAsync_Returns_Data_As_Saved()
        {
            var testData = GenerateTestData();
            var service = GetServiceInstance(StorageFileName);

            await service.SaveDataAsync(testData);

            var data = await service.LoadDataAsync();
            Assert.True(CompareEquality(testData, data));

            await CleanUp();
        }

        [Fact]
        public async Task DeleteDataAsync_Deletes_Data()
        {
            var testData = GenerateTestData();
            var service = GetServiceInstance(StorageFileName);

            await service.SaveDataAsync(testData);
            var data = await service.LoadDataAsync();
            Assert.NotNull(data);

            await service.DeleteDataAsync();
            data = await service.LoadDataAsync();
            Assert.Null(data);

            await CleanUp();
        }

        [Fact]
        public async Task DeleteDataAsync_Works_When_No_Data_Is_Present()
        {
            var service = GetServiceInstance(StorageFileName);
            await service.DeleteDataAsync();
            await service.DeleteDataAsync();

            await CleanUp();
        }

        [Fact]
        public async Task OperationInProgress_Works()
        {
            var testData = GenerateTestData();
            var operationsToTest = new Func<IAppStorageService<TData>, Task>[]
            {
                d => d.SaveDataAsync(testData),
                d => d.LoadDataAsync(),
                d => d.DeleteDataAsync()
            };

            foreach(var i in operationsToTest)
            {
                var testService = GetServiceInstance(StorageFileName);
                Assert.False(testService.OperationInProgress);
                var task = i(testService);
                Assert.True(testService.OperationInProgress);
                await task;
                Assert.False(testService.OperationInProgress);
            }

            await CleanUp();
        }

        private Task CleanUp()
        {
            var service = GetServiceInstance(StorageFileName);
            return service.DeleteDataAsync();
        }
    }
}
