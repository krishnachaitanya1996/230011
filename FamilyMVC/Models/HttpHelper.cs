using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FamilyMVC.Models
{
    public class HttpHelper
    {
        const string Url = "http://localhost:64026/";

        public bool PostData<T>(T oOjbect, string url)
        {
            try
            {
                HttpClient httpclient = new HttpClient();
                string apiurl = Url + url;
                string JSON = JsonConvert.SerializeObject(oOjbect);
                //StringContent content = new StringContent(JSON, Encoding.UTF8, $"application/json");
                HttpContent httpContent = new StringContent(JSON, UnicodeEncoding.UTF8, "application/json");
                httpclient.BaseAddress = new Uri(apiurl);
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.ExpectContinue = false;
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

              
                HttpResponseMessage msg = httpclient.PostAsync(apiurl, httpContent).Result;

                msg.EnsureSuccessStatusCode();
                return msg.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int PostDataAndGetAppId<T>(T oOjbect, string url)
        {
            try
            {
                HttpClient httpclient = new HttpClient();
                string apiurl = Url + url;
                string JSON = JsonConvert.SerializeObject(oOjbect);
                //StringContent content = new StringContent(JSON, Encoding.UTF8, $"application/json");
                HttpContent httpContent = new StringContent(JSON, UnicodeEncoding.UTF8, "application/json");
                httpclient.BaseAddress = new Uri(apiurl);
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.ExpectContinue = false;
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage msg = httpclient.PostAsync(apiurl, httpContent).Result;

                msg.EnsureSuccessStatusCode();
                var data = msg.Content.ReadAsStringAsync();
                int applicationId = int.Parse(data.Result);
                return applicationId;
                //return msg.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<T> GetCall<T>(string url) where T :new()
        {
            T requestedData = new T();
            try
            {
                string apiUrl = Url + url;

                HttpClient httpClient = new HttpClient();

                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;
                //var msg = httpClient.GetAsync(apiUrl);

                if(responseMessage.IsSuccessStatusCode)
                {
                    var data = await responseMessage.Content.ReadAsStringAsync();

                    requestedData = JsonConvert.DeserializeObject<T>(data);

                    return requestedData;
                }
                else
                {
                    return requestedData;
                }
            }
            catch(Exception ex)
            {
                return requestedData;
            }
        }

    }
}