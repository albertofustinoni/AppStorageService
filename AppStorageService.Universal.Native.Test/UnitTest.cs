using AppStorageService.Core.Test.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Threading.Tasks;

namespace AppStorageService.Universal.Native.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestSimpleInstance()
        {
            await TestDataType(TestModel.Generate());
        }

        [TestMethod]
        public async Task TestArray()
        {
            var input = Enumerable.Range(1, 4).Select(d => TestModel.Generate()).ToArray();
            await TestDataType(input);
        }

        [TestMethod]
        public async Task TestList()
        {
            var input = Enumerable.Range(1, 4).Select(d => TestModel.Generate()).ToList();
            await TestDataType(input);
        }

        private async Task TestDataType<T>(T testData) where T : class
        {
            var service = new AppStorageService<T>("Test.json");
            await service.SaveDataAsync(testData);
            var loaded = await service.LoadDataAsync();
            Assert.IsNotNull(loaded);
            await service.DeleteDataAsync();
        }
    }
}
