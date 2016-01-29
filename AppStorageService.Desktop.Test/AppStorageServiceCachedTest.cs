using AppStorageService.Core.Test;

namespace AppStorageService.Desktop.Test
{
    public class AppStorageServiceCachedTest : AppStorageServiceTestBase<AppStorageServiceCached<TestModel>>
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
    }
}
