using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility
{
    public class HttpRequest : IHttpRequest
    {
        public HttpClient client = new HttpClient();

        public string BaseUrl { get; set; }
        private string _token;
        public String Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }
        public HttpRequest(string baseUrl) { BaseUrl = baseUrl; }

        public async Task<T> Get<T>(string url)
        {
            string getUrl = ConbineUrl(url);
            try
            {
                HttpResponseMessage message = await client.GetAsync(getUrl);
                if (message.IsSuccessStatusCode)
                {
                    string content = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
        public async Task<string> Delete(string url)
        {
            string deleteUrl = ConbineUrl(url);
            try
            {
                HttpResponseMessage message = await client.DeleteAsync(deleteUrl);
                if (message.IsSuccessStatusCode)
                {
                    {
                        return "刪除成功！！";
                    }
                }
                return "刪除失敗！！";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<T> Post<T>(string url, object addContent)
        {
            string postUrl = ConbineUrl(url);
            try
            {

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(addContent));
                HttpResponseMessage message = await client.PostAsync(postUrl, stringContent);
                if (message.IsSuccessStatusCode)
                {
                    string content = await message.Content.ReadAsStringAsync();
                    if (content == "")
                        content = message.Headers.Location.ToString();
                    return JsonConvert.DeserializeObject<T>(content);

                }
                return JsonConvert.DeserializeObject<T>("新增失敗！！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public async Task<T> Patch<T>(string url, object patchContent)
        {
            string patchUrl = ConbineUrl(url);

            try
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(patchContent));
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = new HttpMethod("Patch");
                httpRequestMessage.Content = stringContent;
                httpRequestMessage.RequestUri = new Uri(patchUrl);
                HttpResponseMessage message = await client.SendAsync(httpRequestMessage);
                if (message.IsSuccessStatusCode)
                {
                    string content = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);

                }
                return JsonConvert.DeserializeObject<T>("更新失敗！！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
        public async Task<T> Put<T>(string url, object putContent)
        {
            string putUrl = ConbineUrl(url);

            try
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(putContent));
                HttpResponseMessage message = await client.PutAsync(putUrl, stringContent);
                if (message.IsSuccessStatusCode)
                {
                    string content = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);

                }
                return JsonConvert.DeserializeObject<T>("修改失敗！！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public async Task<string> GetAsync(string url)
        {
            string content = await Get<string>(url);
            return content;
        }

        // https://api.youtube.com/video/search?title=台積電&time=weekly
        // baseURL = https://api.youtube.com
        // url = video/search
        // urlParms = title,台積電   time,weekly
        // domain?name=leo&gender=male
        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }

            TResult content = await Get<TResult>(lastUrl);
            return content;
        }

        public async Task<string> PostAsync(string url, object input)
        {
            string result = await Post<string>(url, input);
            return result;
        }

        public async Task<string> PostAsync(string url, object input, Dictionary<string, string> urlParam)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl = lastUrl.TrimEnd('&');
            }
            string postUrl = ConbineUrl(lastUrl);
            try
            {

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(input));
                HttpResponseMessage message = await client.PostAsync(postUrl, stringContent);

                return message.Headers.Location.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl = lastUrl.TrimEnd('&');
            }

            TResult content = await Post<TResult>(lastUrl, input);
            return content;
        }

        public async Task<TResult> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }

            TResult content = await Post<TResult>(lastUrl, input);
            return content;
        }

        public async Task<TResult> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }

            TResult content = await Patch<TResult>(lastUrl, input);
            return content;
        }

        public async Task<TResult> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }

            TResult content = await Patch<TResult>(lastUrl, input);
            return content;
        }

        public async Task<string> PutAsync(string url, object input)
        {
            string result = await Put<string>(url, input);
            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }

            TResult content = await Put<TResult>(lastUrl, input);
            return content;
        }

        public async Task<string> PutAsync(string url, HttpContent content)
        {
            string result = await Put<string>(url, content);
            return result;
        }

        public async Task<string> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        {
            string lastUrl = url;
            if (urlParam != null)
            {
                lastUrl += '?';
                foreach (var pairSet in urlParam)
                {
                    lastUrl = lastUrl + pairSet.Key + '=' + pairSet.Value + '&';
                }
                lastUrl.TrimEnd();
            }
            string result = await Delete(lastUrl);
            return result;
        }

        private string ConbineUrl(string url)
        {
            string completeUrl = BaseUrl + url;
            return completeUrl;
        }
    }
}
