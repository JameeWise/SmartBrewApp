using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBrewApp.Models
{
    /// <summary>
    /// class used for tracking a users drinking habits
    /// </summary>
    internal class BrewerStats : Item // not sure if this needs to inherit from 'Item' but it may make UI integration easier?
    {
        public double AverageTemperature { get; set; }

        public int AverageBrewsPerDay { get; set; }

        public DateTime AverageBrewTime { get; set; }
    }
}
