using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Net.Http.Json;
using FinTracker.Common.Shared.Model;

namespace FinTracker.Utilities.Repository
{
    public static class RepositoryHelper
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public class UrlLookupResult
        {
            public string Message { get; set; }
            public string Url { get; set; }
            public bool Success { get; set; }
        }

        // Get Entity - Generic Method for GET requests that return a single entity
        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetEntity<T>(string url)
        {
            try
            {
                var model = await HttpClient.GetFromJsonAsync<T>(url);
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, HttpStatusCode.OK);
            }
            catch (HttpRequestException ex)
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
                var model = await HttpClient.GetFromJsonAsync<List<T>>(requestUrl);
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, HttpStatusCode.OK);
            }
            catch (HttpRequestException ex)
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
                response.EnsureSuccessStatusCode();

                var model = await response.Content.ReadFromJsonAsync<T>();
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }
        public static async Task<(TResponse model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostEntity<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await HttpClient.PostAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                var model = await response.Content.ReadFromJsonAsync<TResponse>();
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, response.StatusCode);
            }
            catch (HttpRequestException ex)
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
                response.EnsureSuccessStatusCode();

                var model = await response.Content.ReadFromJsonAsync<T>();
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, response.StatusCode);
            }
            catch (HttpRequestException ex)
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
                response.EnsureSuccessStatusCode();

                var model = await response.Content.ReadFromJsonAsync<T>();
                return (model, new UrlLookupResult { Success = true, Message = "Request succeeded." }, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }

        public static async Task<(T model, UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostBankTransaction<T>(string url, Guid id, Stream fileContentStream)
        {
            try
            {

                // Create the multipart form data content
                var multipartContent = new MultipartFormDataContent();

                // Add the file content
                if (fileContentStream != null)
                {
                    var fileContent = new StreamContent(fileContentStream);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = "uploaded_file.csv"
                    };
                    multipartContent.Add(fileContent, "file", "uploaded_file.csv");
                }

                // Send the HTTP POST request
                var response = await HttpClient.PostAsync($"{url}?id={id}", multipartContent);
                response.EnsureSuccessStatusCode();

                // Deserialize the response to the specified type
                var responseModel = await response.Content.ReadFromJsonAsync<T>();
                return (responseModel, new UrlLookupResult { Success = true, Message = "Request succeeded." }, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return (default, new UrlLookupResult { Success = false, Message = ex.Message }, HttpStatusCode.InternalServerError);
            }
        }



    }
}
