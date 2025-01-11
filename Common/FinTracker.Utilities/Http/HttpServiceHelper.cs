using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FinTracker.Utilities.Properties;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Http.HttpMethod;

namespace FinTracker.Utilities.Http
{
    public interface IHttpRequest { }

    public interface IBaseHttpRequest : IHttpRequest { }

    [Serializable]
    public class BaseHttpRequest : BindableObject, IBaseHttpRequest { }

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string Message { get; set; }
    }

    public interface IListBaseHttpResponse<T> : IHttpResponse
    {
        List<T> Result { get; set; }
    }

    public interface IBaseHttpResponse<T> : IHttpResponse
    {
        T Result { get; set; }
    }

    [Serializable]
    public class ListBaseHttpResponse<T> : IListBaseHttpResponse<T>
    {
        public List<T>? Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }

    public class BaseHttpResponse<T> : IBaseHttpResponse<T>
    {
        public T? Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }


    public static class HttpServiceHelper
    {
        public static async Task<IHttpResponse> ProcessHttpRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string url, TRequest request, string? accessToken = null, string tokenName = "Authorization", HttpClient? httpClient = null)
        {
            using (httpClient ??= new HttpClient())
            {

                IHttpResponse? response = null;
                HttpResponseMessage? httpResponse = null;

                if (accessToken != null)
                {
                    httpClient.DefaultRequestHeaders.Add(tokenName, $"Bearer {accessToken}");
                }

                try
                {
                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request)));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    httpResponse = httpMethod switch
                    {
                        { } method when method == Get => await httpClient.GetAsync(url),
                        { } method when method == Post => await httpClient.PostAsync(url, content),
                        { } method when method == Put => await httpClient.PutAsync(url, content),
                        { } method when method == Delete => await httpClient.DeleteAsync(url),
                        _ => throw new ArgumentOutOfRangeException(nameof(httpMethod))
                    };

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var jsonString = await httpResponse.Content.ReadAsStringAsync();
                        var anObject = JsonConvert.DeserializeObject<object>(jsonString);
                        response = anObject switch
                        {
                            JArray jsonObjectArray => new ListBaseHttpResponse<TResponse>
                            {
                                Result = jsonObjectArray.ToObject<List<TResponse>>(),
                                StatusCode = httpResponse.StatusCode,
                                Message = "Success"
                            },
                            JObject jsonObject => new BaseHttpResponse<TResponse>
                            {
                                Result = jsonObject.ToObject<TResponse>(),
                                StatusCode = httpResponse.StatusCode,
                                Message = "Success"
                            },
                            _ => response
                        };
                    }
                    else
                    {
                        return new BaseHttpResponse<TResponse>
                        {
                            StatusCode = httpResponse.StatusCode,
                            Result = default(TResponse),
                            Message = await httpResponse.Content.ReadAsStringAsync()
                        };
                    }
                }
                catch (Exception exception)
                {
                    return new BaseHttpResponse<TResponse>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Result = default(TResponse),
                        Message = exception.Message
                    };
                }
                return response ?? new BaseHttpResponse<TResponse>
                {
                    StatusCode = httpResponse?.StatusCode ?? HttpStatusCode.InternalServerError,
                    Result = default(TResponse),
                    Message = "Response was null, something went wrong"
                };
            }
        }

        public static async Task<IHttpResponse> ProcessHttpRequestAsync<T>(HttpMethod httpMethod, string url, T model, string? accessToken = null, HttpClient? httpClient = null)
        {
            using (httpClient ??= new HttpClient())
            {
                IHttpResponse? response = null;
                HttpResponseMessage? httpResponse = null;

                if (accessToken != null)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);
                }

                try
                {
                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    httpResponse = httpMethod switch
                    {
                        { } method when method == Get => await httpClient.GetAsync(url),
                        { } method when method == Post => await httpClient.PostAsync(url, content),
                        { } method when method == Put => await httpClient.PutAsync(url, content),
                        { } method when method == Delete => await httpClient.DeleteAsync(url),
                        _ => throw new ArgumentOutOfRangeException(nameof(httpMethod))
                    };

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var jsonString = await httpResponse.Content.ReadAsStringAsync();
                        var anObject = JsonConvert.DeserializeObject<object>(jsonString);
                        switch (anObject)
                        {
                            case JArray jsonObjectArray:
                                {
                                    response = new ListBaseHttpResponse<T>
                                    {
                                        Result = jsonObjectArray.ToObject<List<T>>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                            case JObject jsonObject:
                                {
                                    response = new BaseHttpResponse<T>
                                    {
                                        Result = jsonObject.ToObject<T>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                        }
                    }
                    else
                    {
                        response = new BaseHttpResponse<T>
                        {
                            StatusCode = httpResponse.StatusCode,
                            Result = default(T),
                            Message = await httpResponse.Content.ReadAsStringAsync()
                        };
                    }
                }
                catch (Exception exception)
                {
                    response = new BaseHttpResponse<T>
                    {
                        StatusCode = httpResponse?.StatusCode ?? HttpStatusCode.InternalServerError,
                        Result = default(T),
                        Message = exception.Message
                    };
                }
                return response;
            }
        }
        public static bool IsAjaxRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            var headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        public static bool IsApiRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return request.Path.StartsWithSegments(new PathString("/api"));
        }
    }
}
