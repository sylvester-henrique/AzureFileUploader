using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace AzureFileUploader
{
    class Program
    {
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var path = configuration["UploadAbsolutePath"];
            var tempPath = Path.Combine(path, @"..\AzureFileUploaderTemp.zip");
            var connectionString = Environment.GetEnvironmentVariable(configuration["Azure:ConnectionStringEnvironmentVariableName"]);
            var azureBlobContainer = configuration["Azure:BlobContainer"];
            var azureBlobNamePrefix = string.IsNullOrWhiteSpace(configuration["Azure:BlobNamePrefix"]) ? string.Empty : configuration["Azure:BlobNamePrefix"] + "_";
            var azureBlobName = azureBlobNamePrefix + DateTime.Now.ToString(@"yyyy-MM-dd_hh\h-mm\m") + ".zip";

            var beginTime = DateTime.Now;

            Console.WriteLine("Compressing files...");
            if (File.Exists(tempPath))
                File.Delete(tempPath);

            ZipFile.CreateFromDirectory(path, tempPath, CompressionLevel.Fastest, includeBaseDirectory: false);
            Console.WriteLine("Successfully compressed files.");
            var compressTime = DateTime.Now - beginTime;
            Console.WriteLine($"Compress time: {compressTime.ToString(@"hh\:mm\:ss")}");

            var blobContainerClient = new BlobContainerClient(connectionString, azureBlobContainer);
            var blobClient = blobContainerClient.GetBlobClient(azureBlobName);

            var fileInfo = new FileInfo(tempPath);
            var progress = new Progress(fileInfo.Length, pace: 1);

            var beginUpload = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("Uploading...");
            using (var uploadFileStream = fileInfo.OpenRead())
            {
                await blobClient.UploadAsync(uploadFileStream, progressHandler: progress);
            }
            fileInfo.Delete();
            Console.WriteLine();
            Console.WriteLine("Successfully uploaded files.");

            var endTime = DateTime.Now;
            var uploadTime = endTime - beginUpload;
            var totalTime = endTime - beginTime;
            Console.WriteLine($"Upload time: {uploadTime.ToString(@"hh\:mm\:ss")}");
            Console.WriteLine($"Total time:  {totalTime.ToString(@"hh\:mm\:ss")}");

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}