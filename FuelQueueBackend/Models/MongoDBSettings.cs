namespace FuelQueueBackend.Models
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;

        public string FuelStationsCollectionName { get; set; } = null!;

        public string FuelTypesCollectionName { get; set; } = null!;

        public string VehicleTypesCollectionName { get; set; } = null!;

     
    }
}
