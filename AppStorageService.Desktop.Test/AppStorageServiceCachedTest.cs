using System;
using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;

namespace AppStorageService.Desktop.Test
{
    public class AppStorageServiceCachedTest : AppStorageServiceTestBase<AppStorageServiceCached<TestModel>, TestModel>
    {
        private class BackingStore : AppStorageServiceCached<TestModel>
        {
            protected override TestModel CloneData(TestModel input)
            {
                return new TestModel
                {
                    IntProperty = input.IntProperty,
                    StringProperty = input.StringProperty
                };
            }

            public BackingStore(string fileName) : base(fileName) { }
        }

        protected override AppStorageServiceCached<TestModel> GetServiceInstance(string storageFileName)
        {
            return new BackingStore(storageFileName);
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
