using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TweetApi.Models;

public class Tweet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; } = string.Empty;
}