using AppStorageService.Core.Test;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppStorageService.Universal.Test
{
    [TestClass]
    public class AppStorageServiceTest
    {
        const string FileName = "filename.dat";
        public readonly TestModel SampleData = TestModel.Generate();

        private AppStorageService<TestModel> Service { get; set; }

        [TestInitialize]
        public void Init()
        {
            Service = new AppStorageService<TestModel>(FileName);
        }

        [TestMethod]
        public async Task LoadFromStorageAsync_Returns_Null_If_No_Data_Is_Present()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(FileName);
                await file.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
            }

            var data = await Service.LoadDataAsync();
            Assert.IsNull(data);
        }

        [TestMethod]
        public async Task LoadFromStorageAsync_Returns_Data_As_Saved()
        {
            await Service.SaveDataAsync(SampleData);

            var data = await Service.LoadDataAsync();
            Assert.AreEqual(SampleData, data);
        }

        [TestMethod]
        public async Task Shorter_Data_Truncates_Existing_File()
        {
            var testData = "TestString";
            var testDataLong = testData + testData;

            var testService = new AppStorageService<string>(FileName);
            await testService.SaveDataAsync(testDataLong);

            await testService.SaveDataAsync(testData);
            var data = await testService.LoadDataAsync();
            Assert.AreEqual(testData, data);
        }

        private static string ToJSON<T>(T obj) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
