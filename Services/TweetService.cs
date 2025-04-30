using MongoDB.Bson;
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
    
    
    public async Task<List<HashtagCount>> GetTopHashtagsAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument("$unwind", "$entities.hashtags"),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument("$toLower", "$entities.hashtags.text") },
                { "Count", new BsonDocument("$sum", 1) }
            }),
            new BsonDocument("$sort", new BsonDocument("Count", -1)),
            new BsonDocument("$limit", 10)
        };

        var result = await _tweets.Aggregate<HashtagCount>(pipeline).ToListAsync();
        return result;
    }

}

