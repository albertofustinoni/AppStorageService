using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;
using System.Collections.Generic;
using System.Linq;

namespace AppStorageService.Universal.Test
{
    public class AppStorageServiceTestEnum : AppStorageServiceTestBase<AppStorageService<IEnumerable<TestModel>>, IEnumerable<TestModel>>
    {
        protected override AppStorageService<IEnumerable<TestModel>> GetServiceInstance(string storageFileName)
        {
            return new AppStorageService<IEnumerable<TestModel>>(storageFileName);
        }

        protected override IEnumerable<TestModel> GenerateTestData()
        {
            return Enumerable.Range(1, 5).Select(d => TestModel.Generate()).ToArray();
        }

        protected override bool CompareEquality(IEnumerable<TestModel> reference, IEnumerable<TestModel> value)
        {
            return TestModel.EnumEquals(reference, value);
        }
    }
}
