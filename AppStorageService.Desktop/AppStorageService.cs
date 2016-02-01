using AppStorageService.Core;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AppStorageService.Desktop
{
    public class AppStorageService<TData> : AppStorageServiceBase<TData> where TData : class
    {
        public AppStorageService(string fileName) : base(fileName) { }

        protected override async Task<TData> LoadDataAsyncLogic()
        {
            var task = Task.Run<TData>(() =>
            {
                TData output;
                try
                {
                    using (var store = GetStore())
                    {
                        using (var stream = store.OpenFile(FileName, FileMode.Open))
                        {
                            var serializer = new DataContractJsonSerializer(typeof(TData));
                            output = (TData)serializer.ReadObject(stream);
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    return null;
                }
                return output;
            });

            var result = await task;

            return result;
        }

        protected override async Task SaveDataAsyncLogic(TData data)
        {
            var task = Task.Run(() =>
            {
                using (var store = GetStore())
                {
                    using (var stream = store.OpenFile(FileName, FileMode.Create))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(TData));
                        serializer.WriteObject(stream, data);
                    }
                }
            });

            await task;
        }

        protected override async Task DeleteDataAsyncLogic()
        {
            var task = Task.Run(() =>
            {
                using (var store = GetStore())
                {
                    if(store.FileExists(FileName))
                    {
                        store.DeleteFile(FileName);
                    }
                }
            });

            await task;
        }

        private static IsolatedStorageFile GetStore()
        {
            return IsolatedStorageFile.GetUserStoreForAssembly();
        }
    }
}
