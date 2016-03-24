using AppStorageService.Core;
using PCLStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AppStorageService.Xamarin
{
    public class AppStorageService<TData> : AppStorageServiceBase<TData> where TData : class
    {
        public AppStorageService(string fileName) : base(fileName) { }

        protected override async Task<TData> LoadDataAsyncLogic()
        {
            var file = await GetFile(FileName, false);
            if (file == null)
            {
                return null;
            }

            using (var stream = await file.OpenAsync(FileAccess.Read))
            {
                var serializer = new DataContractJsonSerializer(typeof(TData));
                var output = (TData)serializer.ReadObject(stream);
                return output;
            }
        }

        protected override async Task SaveDataAsyncLogic(TData data)
        {
            var file = await GetFile(FileName, true);

            using (var stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                var serializer = new DataContractJsonSerializer(typeof(TData));
                serializer.WriteObject(stream, data);
            }
        }

        protected override async Task DeleteDataAsyncLogic()
        {
            var file = await GetFile(FileName, false);
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

        private static async Task<IFile> GetFile(string fileName, bool createIfNotExists)
        {
            IFile output = null;
            var folder = FileSystem.Current.LocalStorage;
            var existResult = await folder.CheckExistsAsync(fileName);
            if (existResult == ExistenceCheckResult.FileExists)
            {
                output = await folder.GetFileAsync(fileName);
            }
            else if (createIfNotExists)
            {
                output = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }

            return output;
        }
    }
}
