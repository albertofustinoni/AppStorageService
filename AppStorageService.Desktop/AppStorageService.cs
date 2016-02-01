using AppStorageService.Core;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace AppStorageService.Desktop
{
    public class AppStorageService<TData> : AppStorageServiceBase<TData> where TData : class
    {
        public AppStorageService(string fileName) : base(fileName) { }

        protected override async Task<string> LoadDataAsyncLogic()
        {
            string output = null;
            try
            {
                using (var store = GetStore())
                using (var stream = store.OpenFile(FileName, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    output = await reader.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException)
            {

            }
            return output;
        }

        protected override async Task SaveDataAsyncLogic(string serializedData)
        {
            using (var store = GetStore())
            using (var stream = store.OpenFile(FileName, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(serializedData);
            }
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
