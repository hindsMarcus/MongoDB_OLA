using MongoDB.Driver;
using TweetApi.Models;

namespace TweetApi.Services;

public class TweetService
{
    private readonly IMongoCollection<Tweet> _tweets;

    public TweetService(IConfiguration config)
    {
        var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _tweets = database.GetCollection<Tweet>(settings.CollectionName);
    }

    public async Task<List<Tweet>> GetFirst10Async() =>
        await _tweets.Find(_ => true).Limit(10).ToListAsync();

    public async Task CreateAsync(Tweet tweet) =>
        await _tweets.InsertOneAsync(tweet);
}