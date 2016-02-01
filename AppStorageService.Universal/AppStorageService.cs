using AppStorageService.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStorageService.Universal
{
    public class AppStorageService<TData> : AppStorageServiceBase<TData> where TData : class
    {
        public AppStorageService(string fileName) : base(fileName) { }

        protected override async Task SaveDataAsyncLogic(string serializedData)
        {
            var folder = GetStorageFolder();
            var file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (var unprotectedStream = new InMemoryRandomAccessStream())
            using (var writer = new StreamWriter(unprotectedStream.AsStreamForWrite()))
            {
                await writer.WriteAsync(serializedData);
                await writer.FlushAsync();
                unprotectedStream.Seek(0);

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var protector = new DataProtectionProvider("LOCAL=user");
                    await protector.ProtectStreamAsync(unprotectedStream, stream);
                    await stream.FlushAsync();
                }
            }
        }

        protected override async Task<string> LoadDataAsyncLogic()
        {
            string output = null;
            var folder = GetStorageFolder();
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(FileName);
            }
            catch (FileNotFoundException)
            {
                return output;
            }

            using (var unprotectedStream = new InMemoryRandomAccessStream())
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var protector = new DataProtectionProvider();
                    await protector.UnprotectStreamAsync(stream, unprotectedStream);
                }

                unprotectedStream.Seek(0);
                using (var reader = new StreamReader(unprotectedStream.AsStreamForRead()))
                {
                    output = await reader.ReadToEndAsync();
                }
            }

            return output;
        }

        protected override async Task DeleteDataAsyncLogic()
        {
            var folder = GetStorageFolder();
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(FileName);
            }
            catch (FileNotFoundException)
            {
                return;
            }
            await file.DeleteAsync();
        }

        private StorageFolder GetStorageFolder()
        {
            return ApplicationData.Current.LocalFolder;
        }
    }
}
