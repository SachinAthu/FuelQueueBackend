using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FuelQueueBackend.Models
{
    public class Fuel
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [BsonElement("arrival_time")]
        [JsonPropertyName("arrival_time")]
        public string? ArrivalTime { get; set; } = null!;

        [BsonElement("finish_time")]
        [JsonPropertyName("finish_time")]
        public string? FinishTime { get; set; } = null!;

        // true - Available
        // false - Finished
        [BsonElement("status")]
        [JsonPropertyName("status")]
        public bool? Status { get; set; }
    }
}
