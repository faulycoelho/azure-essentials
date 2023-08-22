using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Console.WriteLine("[Starting]");

string storageAccount = Environment.GetEnvironmentVariable("AZURE_ESSENTIALS_STORAGE");
Console.WriteLine($"[Storage Account]:\t {storageAccount}");

BlobServiceClient client = new(
        new Uri($"https://{storageAccount}.blob.core.windows.net"),
        new DefaultAzureCredential());

string blobContainerName = $"mycontainer-{Guid.NewGuid()}";
var containerClient = client.GetBlobContainerClient(blobContainerName);
await containerClient.CreateIfNotExistsAsync();
Console.WriteLine($"[Container ready]:\t{blobContainerName}");

string localPath = "data";
Directory.CreateDirectory(localPath);
string fileName = $"custom-file-{Guid.NewGuid()}.txt";
string localFilePath = Path.Combine(localPath, fileName);
await File.WriteAllTextAsync(localFilePath, "Hello, World!");
Console.WriteLine($"[File created]:\t{localFilePath}");

BlobClient blobClient = containerClient.GetBlobClient(fileName);

await blobClient.UploadAsync(localFilePath, true);
Console.WriteLine($"[Upload completed]:\t{fileName}");

IDictionary<string, string> metadata =
         new Dictionary<string, string>
         {
             { "docType", "documents" },
             { "category", "guidance" }
         };
Console.WriteLine($"[Setting blob metadata]:\t{localFilePath}");
await blobClient.SetMetadataAsync(metadata);


string downloadFilePath = localFilePath.Replace(".txt", "Downloaded.txt");
Console.WriteLine($"[Downloading blob to]:\t {downloadFilePath}");

await blobClient.DownloadToAsync(downloadFilePath);
Console.WriteLine("[Download completed]");
BlobProperties properties = await blobClient.GetPropertiesAsync();

Console.WriteLine("[Blob metadata]:");
foreach (var metadataItem in properties.Metadata)
{
    Console.WriteLine($"\tKey: {metadataItem.Key}\tValue: {metadataItem.Value}"); 
}

Console.Write("[Press any key to begin clean up]");
Console.ReadLine();

Console.WriteLine("[Deleting blob container...]");
await containerClient.DeleteAsync();

Console.WriteLine("[Deleting the local source and downloaded files...]");
File.Delete(localFilePath);
File.Delete(downloadFilePath);

Console.WriteLine("[Done]");