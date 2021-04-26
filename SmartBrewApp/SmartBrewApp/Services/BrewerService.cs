using SmartBrewApp.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SmartBrewApp.Services
{

    /// <summary>
    /// internal class used for making requests to the server (i.e the pi)
    /// to retrieve brewer information
    /// </summary>
    public class BrewerService 
    {
        // base URL of the API, subject to change based on the API project
        // should be in the form http://<pi-IP>:<port>
        private readonly string _baseURL = @"http://10.0.1.167/api/smartBrewerapi/";

        private readonly HttpClient _client;

        public BrewerService()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(400) // ~ 6.5 minutes
            };

            _client.DefaultRequestHeaders.Add("Accept", "application/json");

        }

        /// <summary>
        /// checks if we can connect with the API
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyConnection()
        {
            string response = await _client.GetStringAsync(_baseURL).ConfigureAwait(false);
            bool.TryParse(response, out bool isConnected);
            return isConnected;
        }

        /// <summary>
        /// Verify python is running sucessfully on the server
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckPython()
        {
            Uri endpoint = new Uri(_baseURL + "python");
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);

            return response.Length > 0;
        }

        public async Task<string> StartNewBrew(int userId, int desiredCups = 12)
        {
            Uri endpoint = new Uri(_baseURL + $"Brew/{userId}");

            StringContent content = new StringContent(JsonConvert.SerializeObject(desiredCups), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(endpoint, content).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch the current temperature of the brew
        /// </summary>
        /// <returns></returns>
        public async Task<double> ReadTemperature(bool serveTemp = true)
        {
            // create Uri for the desired endpoint
            Uri endpoint = new Uri(_baseURL + $"Temp/{serveTemp}");

            // fetch the temperature through a GET request 
            // returns a string (JSON) response
            //that we can parse for the temperature
            string response = await _client.GetStringAsync(endpoint).ConfigureAwait(false);

            double.TryParse(response.Trim(), out double temp);

            return temp;
        }

        public async Task<bool> CheckWaterLevel()
        {
            Uri endpoint = new Uri(_baseURL + "waterLevel");
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
