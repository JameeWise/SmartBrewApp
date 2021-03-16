using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBrewApp.Models
{
    internal class BrewerStats
    {
        public double AverageTemperature { get; set; }

        public int AverageBrewsPerDay { get; set; }

        public DateTime AverageBrewTime { get; set; }
    }
}
