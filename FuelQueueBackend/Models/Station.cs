using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace FuelQueueBackend.Models
{
    public class Station
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [BsonElement("address")]
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        // Ceypetco (ceypetco) or IOC (ioc)
        [BsonElement("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [BsonElement("fuel")]
        [JsonPropertyName("fuel")]
        public Fuel? Fuel { get; set; } = null!;

        public Station()
        {
        }

        public Station(string name, string address, string type)
        {
            Name = name;
            Address = address;
            Type = type;
        }
    }
}
