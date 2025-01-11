//using System.Net;
//using FinTracker.Utilities.Extensions;
//using FinTracker.Utilities.Helper;
//using FinTracker.Utilities.Logging;

//namespace FinTracker.Utilities.Http
//{
//    public class HttpRequest<TResult> : HttpRequest
//    {
//        public HttpRequest(string url) : base(url) { }
//        public TResult Result;
//    }

//    public class HttpRequest
//    {
//        public HttpRequest(string url)
//        {
//            Url = url;
//        }

//        public HttpStatusCode? HttpStatusCode;
//        public string HttpResponseContent;
//        public bool Successful;
//        public string Url;
//    }


//    public static class HttpService
//    {
//        public static async Task<HttpRequest<T>> DeleteAsync<T>(string url, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, T>(HttpMethod.Delete, url, default, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<TResult>> DeleteAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, TResult>(HttpMethod.DeleteWithReturn, url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<T>> GetAsync<T>(string url, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, T>(HttpMethod.Get, url, default, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest> PostAsync(string url, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<object, object>(HttpMethod.Post, url, null, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<T>> PostAsync<T>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, T>(HttpMethod.Post, url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<TResult>> PostAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, TResult>(HttpMethod.PostWithReturn, url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<T>> PutAsync<T>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, T>(HttpMethod.Put, url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        public static async Task<HttpRequest<TResult>> PutAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            return await ProcessRequestAsync<T, TResult>(HttpMethod.PutWithReturn, url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
//        }

//        private static async Task<HttpRequest<TResult>> ProcessRequestAsync<T, TResult>(HttpMethod httpMethod, string url, T obj = default, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
//        {
//            if (httpClient == null)
//            {
//                if (HttpClient != null)
//                {
//                    // We are using the global client for http requests therefore for thread safety we need to
//                    // wait and release the semaphore as only 1 thread at a time should use this publically static client.
//                    await __SemaphoreSlim.WaitAsync();
//                    httpClient = HttpClient;
//                }
//                else
//                {
//                    httpClient = new HttpClient();
//                }
//            }

//            var httpRequest = new HttpRequest<TResult>(url);
//            HttpResponseMessage httpResponseMessage = null;

//            try
//            {
//                if (RequestAuthTokenAsync != null)
//                    authorizationToken = await RequestAuthTokenAsync.Invoke();

//                httpClient.ConfigureAuthorization(authorizationToken, authorizationScheme);

//                switch (httpMethod)
//                {
//                    case HttpMethod.Delete:
//                    case HttpMethod.DeleteWithReturn:
//                        httpResponseMessage = await httpClient.DeleteAsync(url);
//                        break;

//                    case HttpMethod.Get:
//                        httpResponseMessage = await httpClient.GetAsync(url);
//                        break;

//                    case HttpMethod.Post:
//                    case HttpMethod.PostWithReturn:
//                        if (obj != null)
//                            httpResponseMessage = await httpClient.PostAsync(url, HttpHelper.ConvertToByteArrayContent<T>(obj));
//                        else
//                            httpResponseMessage = await httpClient.PostAsync(url, null);
//                        break;

//                    case HttpMethod.Put:
//                    case HttpMethod.PutWithReturn:
//                        httpResponseMessage = await httpClient.PutAsync(url, HttpHelper.ConvertToByteArrayContent<T>(obj));
//                        break;
//                }

//                httpRequest.HttpResponseContent = await httpResponseMessage.GetContent(url, enableHttpResponseLog);
//                httpRequest.HttpStatusCode = httpResponseMessage.StatusCode;
//                httpRequest.Successful = httpResponseMessage.IsSuccessStatusCode;

//                if (httpRequest.Successful && (httpMethod == HttpMethod.Get || httpMethod == HttpMethod.DeleteWithReturn || httpMethod == HttpMethod.PostWithReturn || httpMethod == HttpMethod.PutWithReturn) && typeof(TResult) != typeof(string))
//                {
//                    if (SerializationHelper.DeserializeFromJsonString<TResult>(httpRequest.HttpResponseContent, out TResult resultObj))
//                        httpRequest.Result = resultObj;                                            
//                }
//            }
//            catch (Exception exception)
//            {                                
//                LoggingService.LogErrorMessage(exception, $"HttpService.ProcessRequestAsync<{typeof(T).FullName}>({httpMethod})");
//                httpRequest.Successful = false;
//                httpRequest.HttpResponseContent = exception.Message;
//            }
//            finally
//            {
//                if (HttpClient != null)
//                    __SemaphoreSlim.Release();
//            }

//            return httpRequest;
//        }

//        private enum HttpMethod
//        {
//            Delete = 0,

//            DeleteWithReturn = 1,

//            Get = 2,

//            Post = 3,

//            PostWithReturn = 4,

//            Put = 5,

//            PutWithReturn = 6
//        }

//        private static SemaphoreSlim __SemaphoreSlim = new SemaphoreSlim(1, 1);

        

//        public static HttpClient HttpClient;

//        public delegate Task<string> RequestAuthTokenEventHandler();

//        /// <summary>
//        /// An action that requests an authorization token through the function passed to the action.
//        /// This action will take precedence over any token already set on the global http client.
//        /// </summary>
//        public static event RequestAuthTokenEventHandler RequestAuthTokenAsync;
//    }
//}
