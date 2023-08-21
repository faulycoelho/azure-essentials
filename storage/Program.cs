using Azure.Identity;
using Azure.Storage.Blobs;

Console.WriteLine("[Starting]");

string storageAccount = Environment.GetEnvironmentVariable("AZURE_ESSENTIALS_STORAGE");
Console.WriteLine($"[Storage Account]:\t {storageAccount}");

BlobServiceClient client = new(
        new Uri($"https://{storageAccount}.blob.core.windows.net"),
        new DefaultAzureCredential());

string blobContainerName = "mycontainer-az";
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

string downloadFilePath = localFilePath.Replace(".txt", "Downloaded.txt");
Console.WriteLine($"[Downloading blob to]:\t {downloadFilePath}");

await blobClient.DownloadToAsync(downloadFilePath);
Console.WriteLine("[Download completed]");

Console.Write("[Press any key to begin clean up]");
Console.ReadLine();

Console.WriteLine("[Deleting blob container...]");
await containerClient.DeleteAsync();

Console.WriteLine("[Deleting the local source and downloaded files...]");
File.Delete(localFilePath);
File.Delete(downloadFilePath);

Console.WriteLine("[Done]");