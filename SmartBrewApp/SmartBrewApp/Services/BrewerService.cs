using SmartBrewApp.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SmartBrewApp.Services
{
    /** *NOTE* Currently, these methods are all untested and dependent on the SmartBrewer.API */


    /** Functions still needed (not a complete list and all are subject for discussion)
     *  GET Brew status
     *  likely several more I am missing 
     *  most of these likely need a corresponding endpoint in the API as well
     */

    /// <summary>
    /// internal class used for making requests to the server (i.e the pi)
    /// to retrieve brewer information
    /// </summary>
    internal class BrewerService 
    {
        // base URL of the API, subject to change based on the API project
        // should be in the form http://<pi-IP>:<port>
        private readonly string _baseURL = @"http://localhost:5000/api/smartBrewerapi";

        private readonly HttpClient _client;

        internal BrewerService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseURL}/")
            };

        }

        public async Task<bool> StartNewBrew(int userId)
        {
            Uri endpoint = new Uri(_client.BaseAddress + "Brew");

            string serializedData = JsonConvert.SerializeObject(userId);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);

            HttpResponseMessage response = await _client.PostAsync(endpoint, byteContent).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Fetch the current temperature of the brew
        /// </summary>
        /// <returns></returns>
        public async Task<double> ReadTemperature()
        {
            // create Uri for the desired endpoint
            Uri endpoint = new Uri(_client.BaseAddress + "brewTemp");

            // fetch the temperature through a GET request 
            // returns a string (JSON) response
            //that we can parse for the temperature
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

        #region User Based Settings

        public async Task<User> GetCurrentUser(int id)
        {
            Uri endpoint = new Uri($"{_client.BaseAddress}User/{id}");
            
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);
            User user = JsonConvert.DeserializeObject<User>(response);

            return user;

        }

        public async Task<Preferences> GetUserPreferences(int userId)
        {
            Uri endpoint = new Uri($"{_client.BaseAddress}Preferences/{userId}");
            
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);
            Preferences userPrefs = JsonConvert.DeserializeObject<Preferences>(response);

            return userPrefs;

        }

        public async Task<bool> UpdatePreferences(Preferences prefs)
        {

            Uri endpoint = new Uri(_client.BaseAddress + "Preferences");

            // to pass data we need to serialize it into a JSON string
            // then add it to a buffer to send over 
            string serializedData = JsonConvert.SerializeObject(prefs);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);

            // make the request and wait the response
            HttpResponseMessage response = await _client.PutAsync(endpoint, byteContent).ConfigureAwait(false);


            // simply return if the request was successful
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


        #endregion
    }
}
