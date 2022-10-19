using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FuelQueueBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(pattern: "^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [BsonElement("password")]
        [JsonPropertyName("password")]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long")]
        [RegularExpression(pattern: "^\\S{8,8}\\S*$", ErrorMessage = "Password is too weak")]
        public string Password { get; set; } = null!;

        // station owner (station_owner) or user (user)
        [BsonElement("type")]
        [JsonPropertyName("type")]
        [Required(ErrorMessage = "User type is required (Station Owner or User)")]
        public string Type { get; set; } = null!;



    }
}
