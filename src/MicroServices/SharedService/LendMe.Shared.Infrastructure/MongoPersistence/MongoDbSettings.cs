namespace LendMe.Shared.Infrastructure.MongoPersistence;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}