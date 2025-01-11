using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FinTracker.Utilities.Http
{
    public static class HttpHelper
    {
        public static ByteArrayContent ConvertToByteArrayContent<T>(T obj)
        {
            var jsonItem = JsonConvert.SerializeObject(obj);
            var bytesJsonItem = Encoding.UTF8.GetBytes(jsonItem);
            var byteArrayContent = new ByteArrayContent(bytesJsonItem);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteArrayContent;
        }
    }
}
