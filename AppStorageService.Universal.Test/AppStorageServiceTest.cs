using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;

namespace AppStorageService.Universal.Test
{
    public class AppStorageServiceTest : AppStorageServiceTestBase<AppStorageService<TestModel>, TestModel>
    {
        protected override AppStorageService<TestModel> GetServiceInstance(string storageFileName)
        {
            return new AppStorageService<TestModel>(storageFileName);
        }

        protected override TestModel GenerateTestData()
        {
            return TestModel.Generate();
        }

        protected override bool CompareEquality(TestModel reference, TestModel value)
        {
            return reference.Equals(value);
        }
    }
}
