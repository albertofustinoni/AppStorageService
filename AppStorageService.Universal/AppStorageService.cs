using AppStorageService.Core;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStorageService.Universal
{
    public class AppStorageService<TData> : AppStorageServiceBase<TData> where TData : class
    {
        public AppStorageService(string fileName) : base(fileName) { }

        public override async Task SaveDataAsync(TData data)
        {
            OperationInProgress = true;

            var folder = GetStorageFolder();
            var file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (var unprotectedStream = new InMemoryRandomAccessStream())
            {
                var serializer = new DataContractJsonSerializer(data.GetType());
                serializer.WriteObject(unprotectedStream.AsStreamForWrite(), data);
                unprotectedStream.Seek(0);

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var protector = new DataProtectionProvider("LOCAL=user");
                    await protector.ProtectStreamAsync(unprotectedStream, stream);
                    await stream.FlushAsync();
                }
            }

            OperationInProgress = false;
        }

        public override async Task<TData> LoadDataAsync()
        {
            OperationInProgress = true;

            var folder = GetStorageFolder();
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(FileName);
            }
            catch (FileNotFoundException)
            {
                return default(TData);
            }

            TData output;

            using (var unprotectedStream = new InMemoryRandomAccessStream())
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var protector = new DataProtectionProvider();
                    await protector.UnprotectStreamAsync(stream, unprotectedStream);
                }

                unprotectedStream.Seek(0);
                var serializer = new DataContractJsonSerializer(typeof(TData));
                output = (TData)serializer.ReadObject(unprotectedStream.AsStreamForRead());
            }

            OperationInProgress = false;
            return output;
        }

        public override async Task DeleteDataAsync()
        {
            OperationInProgress = true;

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

            OperationInProgress = false;
        }

        private StorageFolder GetStorageFolder()
        {
            return ApplicationData.Current.LocalFolder;
        }
    }
}
