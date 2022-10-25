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

        [BsonElement("full_name")]
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; } = null!;

        [BsonElement("mobile_number")]
        [JsonPropertyName("mobile_number")]
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(pattern: "^0[0-9]{9,9}$", ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; } = null!;

        //[BsonElement("email")]
        //[JsonPropertyName("email")]
        //[Required(ErrorMessage = "Email is required")]
        //[RegularExpression(pattern: "^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$", ErrorMessage = "Invalid email address")]
        //public string Email { get; set; } = null!;

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
        public string UserType { get; set; } = null!;

        [BsonElement("vehicle_type")]
        [JsonPropertyName("vehicle_type")]
        public string? VehicleType { get; set; } = null!;

        [BsonElement("station")]
        [JsonPropertyName("station")]
        public Station? Station { get; set; } = null!;

        public string? Token { get; set; } = null!;


        public User()
        {
           
        }

        public User(string? id, string mobileNumber, string userType)
        {
            Id = id;
            MobileNumber = mobileNumber;
            UserType = userType;
        }

        public User(string? id, string? fullName, string mobileNumber, string password, string userType, string? vehicleType) : this(id, fullName, mobileNumber)
        {
            Password = password;
            UserType = userType;
            VehicleType = vehicleType;
        }
    }
}
