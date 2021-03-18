using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBrewApp.Models
{
    /// <summary>
    /// Class for storing a user's drinking preferences
    /// </summary>
    public class Preferences : Item // not sure if this needs to inherit from 'Item' but it may make UI integration easier?
    {
        public double DrinkingTemperature { get; set; } = 195;

        public double ServingTemperature { get; set; } = 205;

        public List<DateTime> BrewTimes { get; } = new List<DateTime>();
    }
}
