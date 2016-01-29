using System;
using System.Threading.Tasks;
using Xunit;

namespace AppStorageService.Core.Test
{
    [Collection("Collection1")]
    public abstract class AppStorageServiceTestBase<TService> where TService: IAppStorageService<TestModel>
    {
        private const string StorageFileName = "FileName.dat";

        private static readonly TestModel SampleData = TestModel.Generate();

        protected abstract TService GetServiceInstance(string storageFileName);

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
            var service = GetServiceInstance(StorageFileName);

            await service.SaveDataAsync(SampleData);

            var data = await service.LoadDataAsync();
            Assert.Equal(SampleData, data);

            await CleanUp();
        }

        [Fact]
        public async Task DeleteDataAsync_Deletes_Data()
        {
            var service = GetServiceInstance(StorageFileName);

            await service.SaveDataAsync(SampleData);
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
        public async Task Shorter_Data_Truncates_Existing_File()
        {
            /*var testData = "TestString";
            var testDataLong = testData + testData;

            var testService = GetServiceInstance<string>(StorageFileName);
            await testService.SaveDataAsync(testDataLong);

            await testService.SaveDataAsync(testData);
            var data = await testService.LoadDataAsync();
            Assert.Equal(testData, data);*/
        }

        [Fact]
        public async Task OperationInProgress_Works()
        {
            var operationsToTest = new Func<IAppStorageService<TestModel>, Task>[]
            {
                d => d.SaveDataAsync(SampleData),
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
