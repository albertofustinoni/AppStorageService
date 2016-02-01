﻿using AppStorageService.Core.Test;
using AppStorageService.Core.Test.Models;

#if WINDOWS_UWP
namespace AppStorageService.Universal.Test
#else
namespace AppStorageService.Desktop.Test
#endif
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
