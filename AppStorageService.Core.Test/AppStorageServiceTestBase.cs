using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppStorageService.Core.Test
{
    public abstract class AppStorageServiceTestBase<TService> where TService: IAppStorageService<TestModel>
    {
        const string StorageFileName = "FileName.dat";
        public static readonly TestModel SampleData = TestModel.Generate();

        protected abstract IAppStorageService<T> GetServiceInstance<T>(string storageFileName) where T : class;
        protected abstract Task DeleteStorageFile();

        private IAppStorageService<TestModel> Service { get; set; }

        private void Init()
        {
            Service = GetServiceInstance<TestModel>(StorageFileName);
        }

        [Fact]
        public async Task LoadFromStorageAsync_Returns_Null_If_No_Data_Is_Present()
        {
            try
            {
                await DeleteStorageFile();
            }
            catch (FileNotFoundException)
            {
            }

            var data = await Service.LoadDataAsync();
            Assert.Null(data);
        }

        [Fact]
        public async Task LoadFromStorageAsync_Returns_Data_As_Saved()
        {
            await Service.SaveDataAsync(SampleData);

            var data = await Service.LoadDataAsync();
            Assert.Equal(SampleData, data);
        }

        [Fact]
        public async Task Shorter_Data_Truncates_Existing_File()
        {
            var testData = "TestString";
            var testDataLong = testData + testData;

            var testService = GetServiceInstance<string>(StorageFileName);
            await testService.SaveDataAsync(testDataLong);

            await testService.SaveDataAsync(testData);
            var data = await testService.LoadDataAsync();
            Assert.Equal(testData, data);
        }

        private static string ToJSON<T>(T obj) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                var byteArr = stream.ToArray();
                return Encoding.UTF8.GetString(byteArr, 0, byteArr.Length);
            }
        }
    }
}
