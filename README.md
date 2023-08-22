# azure-essentials

# storage
Firstly, specify your environment variable AZURE_ESSENTIALS_STORAGE.

In the Portal Azure go to the storage resource: Access Control (IAM) > Add > Add role assignment > Role = 'Storage Blob Data Contributor' > Members > Select members "select your user" > Review + assign

In the command, navigate to the project directory and run the following commands:

$ az login
$ dotnet build
$ dotnet run
