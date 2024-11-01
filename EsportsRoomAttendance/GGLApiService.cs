using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EsportsRoomAttendance
{
    public class GGLApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _authToken;
        private string _jwtToken;
        private DateTime _tokenExpiryTime;

        public GGLApiService(string authToken)
        {
            _authToken = authToken;
            _httpClient = new HttpClient
            {
                //BaseAddress = new Uri("https://api.ggleap.com/beta/")
            };
        }

        public async Task<bool> UpsertUserAsync(string correlationId, string username, string firstName, string lastName, string uuid = null)
        {
            var url = "/users/upsert";

            // Construct the minimal user object required by the API
            var user = new
            {
                Uuid = uuid,
                Username = username,
                FirstName = firstName,
                LastName = lastName
            };

            var jsonContent = JsonConvert.SerializeObject(new { User = user });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Add the X-Correlation-Id header
            _httpClient.DefaultRequestHeaders.Remove("X-Correlation-Id");
            _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("User upserted successfully.");
                return true;
            }

            Console.WriteLine($"Failed to upsert user: {response.ReasonPhrase}");
            return false;
        }

        /// <summary>
        /// Method to request the JWT. This makes the initial request
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AuthenticateAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.ggleap.com/beta/authorization/public-api/auth")
            {
                Content = new StringContent($"{{\"AuthToken\": \"{_authToken}\"}}", System.Text.Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Parse JSON and retrieve the JWT token
                var json = JObject.Parse(responseBody);
                if (json["Jwt"] != null)
                {
                    _jwtToken = json["Jwt"].ToString();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
                    return true;
                }
                else
                {
                    // Log the full response if "Jwt" is missing
                    Console.WriteLine("JWT token not found in response.");
                    Console.WriteLine("Response: " + responseBody);
                }
            }
            else
            {
                // Log error status and reason for failed request
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + errorResponse);
            }

            return false;
        }

        /// <summary>
        /// Refresh the JWT every 5 minutes
        /// Use a timer to refresh the JWT every 5 minutes to avoid interruptions.
        /// </summary>
        /// <returns></returns>
        public async Task EnsureJwtIsValidAsync()
        {
            if (DateTime.UtcNow >= _tokenExpiryTime.AddMinutes(-5))
            {
                await AuthenticateAsync();
            }
        }

        /// <summary>
        /// Call EnsureJwtIsValidAsync() before any API request to make sure you have a valid JWT.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> MakeAuthenticatedRequestAsync(HttpMethod method, string endpoint)
        {
            await EnsureJwtIsValidAsync();
            var request = new HttpRequestMessage(method, endpoint);
            return await _httpClient.SendAsync(request);
        }
    }
}
