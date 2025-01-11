using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace FinTracker.Utilities.Repository
{
    public static class RepositoryHelper
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        // URL Lookup Result for consistent error handling
        public class UrlLookupResult
        {
            public string Message { get; set; }
            public bool Success { get; set; }
        }

        // Get Entity - Generic Method for GET requests that return a single entity
        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetEntity<T>(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<T>(content);

                return (model, new UrlLookupResult { Success = response.IsSuccessStatusCode, Message = content }, response.StatusCode);
            }
            catch (Exception ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }

        // Get Entity List - Generic Method for GET requests that return a list of entities
        public static async Task<(List<T> model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetEntityList<T>(string url, string queryString = "")
        {
            try
            {
                var requestUrl = string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
                var response = await HttpClient.GetAsync(requestUrl);
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<List<T>>(content);

                return (model, new UrlLookupResult { Success = response.IsSuccessStatusCode, Message = content }, response.StatusCode);
            }
            catch (Exception ex)
            {
                return (null, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }

        // Post Entity - Generic Method for POST requests
        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostEntity<T>(string url, T request)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, jsonContent);
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<T>(content);

                return (model, new UrlLookupResult { Success = response.IsSuccessStatusCode, Message = content }, response.StatusCode);
            }
            catch (Exception ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }

        // Put Entity - Generic Method for PUT requests
        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PutEntity<T>(string url, Guid id, T request)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await HttpClient.PutAsync($"{url}/{id}", jsonContent);
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<T>(content);

                return (model, new UrlLookupResult { Success = response.IsSuccessStatusCode, Message = content }, response.StatusCode);
            }
            catch (Exception ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }

        // Delete Entity - Generic Method for DELETE requests
        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> DeleteEntity<T>(string url, Guid id)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{url}/{id}");
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<T>(content);

                return (model, new UrlLookupResult { Success = response.IsSuccessStatusCode, Message = content }, response.StatusCode);
            }
            catch (Exception ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }
    }
}
