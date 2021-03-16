using SmartBrewApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SmartBrewApp.Services
{
    internal class BrewerService 
    {
        private readonly string _baseURL = @"http://localhost:5000/api/smartBrewer";

        private readonly HttpClient _client;

        internal BrewerService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseURL}/")
            };

        }

        public async Task<double> ReadTemperature()
        {
            Uri endpoint = new Uri(_client.BaseAddress + "brewTemp");
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);

            double.TryParse(response.Trim(), out double temp);

            return temp;
        }

        public async Task<bool> CheckWaterLevel()
        {
            Uri endpoint = new Uri(_client.BaseAddress + "waterLevel");
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);

            bool.TryParse(response.Trim(), out bool isLow);

            return isLow;
        }

        public async Task<bool> UpdatePreferences(Preferences prefs)
        {

            Uri endpoint = new Uri(_client.BaseAddress + "Preferences");

            string serializedData = JsonConvert.SerializeObject(prefs);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);

            HttpResponseMessage response = await _client.PutAsync(endpoint, byteContent).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPreferences()
        {
            Uri endpoint = new Uri(_client.BaseAddress + "preferences");

            HttpResponseMessage response = await _client.DeleteAsync(endpoint).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetStats()
        {
            Uri endpoint = new Uri(_client.BaseAddress + "stats");

            HttpResponseMessage response = await _client.DeleteAsync(endpoint).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetUserData()
        {
            Uri endpoint = new Uri(_client.BaseAddress + "userData");

            HttpResponseMessage response = await _client.DeleteAsync(endpoint).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
