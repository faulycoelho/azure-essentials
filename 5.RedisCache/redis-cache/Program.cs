using StackExchange.Redis;

//az group create --name az204-redis-rg --location BrazilSouth
//
//redisName=az204redis$RANDOM
//az redis create --location BrazilSouth \
//    --resource-group az204-redis-rg \
//    --name $redisName \
//    --sku Basic --vm-size c0
//
//Portal Azure Redis > Access Keys > Primary Conenction string (StackExchange.Redis)

// connection string to your Redis Cache    
string connectionString = "REDIS_CONNECTION_STRING";


using (var cache = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase db = cache.GetDatabase();

    // Snippet below executes a PING to test the server connection
    var result = await db.ExecuteAsync("ping");
    Console.WriteLine($"PING = {result.Type} : {result}");

    // Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
    bool setValue = await db.StringSetAsync("test:key", "100");
    Console.WriteLine($"SET: {setValue}");

    // StringGetAsync retrieves the value for the "test" key
    string getValue = await db.StringGetAsync("test:key");
    Console.WriteLine($"GET: {getValue}");
}