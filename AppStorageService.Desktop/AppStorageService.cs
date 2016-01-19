using AppStorageService.Core;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AppStorageService.Desktop
{
    public class AppStorageService<TData> : IAppStorageService<TData> where TData : class
    {
        public readonly string FileName;
        public bool OperationInProgress { get; private set; }

        public AppStorageService(string fileName)
        {
            FileName = fileName;
            OperationInProgress = false;
        }

        public async Task<TData> LoadDataAsync()
        {
            OperationInProgress = true;
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
            OperationInProgress = false;

            return result;
        }

        public async Task SaveDataAsync(TData data)
        {
            OperationInProgress = true;
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
            OperationInProgress = false;
        }

        private static IsolatedStorageFile GetStore()
        {
            return IsolatedStorageFile.GetUserStoreForAssembly();
        }
    }
}
