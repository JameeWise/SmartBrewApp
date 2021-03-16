using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBrewApp.Models
{
    public class Preferences
    {
        public double DrinkingTemperature { get; set; } = 195;

        public double ServingTemperature { get; set; } = 205;

        public List<DateTime> BrewTimes { get; } = new List<DateTime>();
    }
}
