using DataStax.AstraDB.DataApi.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DataStax.AstraDB.DataApi.IntegrationTests.Fixtures;

[CollectionDefinition("Database")]
public class Databaseollection : ICollectionFixture<DatabaseFixture>
{

}

public class DatabaseFixture
{
    public DataApiClient Client { get; private set; }
    public DataApiClient ClientWithoutToken { get; private set; }
    public Database Database { get; private set; }
    public string OpenAiApiKey { get; set; }
    public string DatabaseUrl { get; set; }
    public string Token { get; set; }

    public DatabaseFixture()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables(prefix: "ASTRA_DB_")
            .Build();

        Token = configuration["TOKEN"] ?? configuration["AstraDB:Token"];
        DatabaseUrl = configuration["URL"] ?? configuration["AstraDB:DatabaseUrl"];
        OpenAiApiKey = configuration["OPENAI_APIKEYNAME"];

        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddFileLogger("../../../_logs/database_fixture_latest_run.log"));
        ILogger logger = factory.CreateLogger("IntegrationTests");

        var clientOptions = new CommandOptions
        {
            RunMode = RunMode.Debug
        };
        Client = new DataApiClient(Token, clientOptions, logger);
        var clientWithoutTokenOptions = new CommandOptions
        {
            RunMode = RunMode.Debug
        };
        ClientWithoutToken = new DataApiClient(null, clientWithoutTokenOptions, logger);
        Database = Client.GetDatabase(DatabaseUrl);
    }

}