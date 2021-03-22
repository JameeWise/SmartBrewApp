using SmartBrewApp.Models;

namespace SmartBrewApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string GeoCoordinate { get; set; }
        
        public Preferences BrewerPreferences { get; set; } = new Preferences();

        public BrewerStats UserStatistics { get; set; } = new BrewerStats();
    }
}