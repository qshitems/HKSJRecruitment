using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Common.Mvc;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Common.Mvc
{
    public class WcfRestClient
    {
        public string ErrMsg { private set; get; }
        private Log log;
        public WcfRestClient()
        {
            log = new Log(typeof(WcfRestClient));
        }
        public WcfRestClient(string baseUrl, string userName, string pwd)
        {
            this.BaseUrl = baseUrl;
            this.UserName = UserName;
            this.Pwd = pwd;
        }
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public TResult Get<TResult>(string path) where TResult : class
        {
            try
            {
                string url = Path.Combine(BaseUrl, path);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                log.Debug(string.Format("Reqeust:Method={0},Url={1}", httpRequest.Method, url));
                InitHttpRequest(httpRequest);
                HttpWebResponse httResp = (HttpWebResponse)httpRequest.GetResponse();
                string result = new StreamReader(httResp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                log.Debug("Response:" + result);
                return JsonConvert.DeserializeObject<TResult>(result, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            }
            catch (System.Exception exp)
            {
                log.Info("Exception:" + exp.ToString());
                ErrMsg = exp.ToString();
            }
            return null;
        }
        public TResult Post<TModel, TResult>(TModel model)
            where TModel : class
            where TResult : class
        {
            return Send<TModel, TResult>(string.Empty, "POST", model);
        }
        public TResult Post<TModel, TResult>(string path, TModel model)
            where TModel : class
            where TResult : class
        {
            return Send<TModel, TResult>(path, "POST", model);
        }
        public TResult Put<TModel, TResult>(TModel model)
            where TModel : class
            where TResult : class
        {
            return Send<TModel, TResult>(string.Empty, "PUT", model);
        }
        public TResult Put<TModel, TResult>(string path, TModel model)
            where TModel : class
            where TResult : class
        {
            return Send<TModel, TResult>(path, "PUT", model);
        }
        public TResult Send<TModel, TResult>(string path, string method, TModel model)
            where TModel : class
            where TResult : class
        {
            try
            {
                string url = Path.Combine(BaseUrl, path);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = method;
                log.Debug(string.Format("Reqeust:Method={0},Url={1}", httpRequest.Method, url));
                InitHttpRequest(httpRequest);
                string modelStr = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
                log.Debug("Data:" + modelStr);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(modelStr);
                Stream reqeustStream = httpRequest.GetRequestStream();
                reqeustStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse httResp = (HttpWebResponse)httpRequest.GetResponse();
                string result = new StreamReader(httResp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                log.Debug("Response:" + result);
                return JsonConvert.DeserializeObject<TResult>(result, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            }
            catch (System.Exception exp)
            {
                log.Info("Exception:" + exp.ToString());
                ErrMsg = exp.ToString();
            }
            return null;
        }
        public TResult Send<TResult>(string path, string method) where TResult : class
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(Path.Combine(BaseUrl, path));
                httpRequest.Method = method;
                InitHttpRequest(httpRequest);
                HttpWebResponse httResp = (HttpWebResponse)httpRequest.GetResponse();
                string result = new StreamReader(httResp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                return JsonConvert.DeserializeObject<TResult>(result, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            }
            catch (System.Exception exp)
            {
                ErrMsg = exp.ToString();
            }
            return null;
        }

        public TResult Delete<TModel,TResult>(string path,TModel model)
            where TModel : class
            where TResult : class
        {
            return Send<TModel, TResult>(path, "DELETE", model);
        }
        public TResult Delete<TResult>(string path) where TResult : class
        {
            try
            {
                string url = Path.Combine(BaseUrl, path);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "DELETE";
                log.Debug(string.Format("Reqeust:Method={0},Url={1}", httpRequest.Method, url));
                InitHttpRequest(httpRequest);
                HttpWebResponse httResp = (HttpWebResponse)httpRequest.GetResponse();
                string result = new StreamReader(httResp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                log.Debug("Response:" + result);
                return JsonConvert.DeserializeObject<TResult>(result, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            }
            catch (System.Exception exp)
            {
                log.Info("Exception:" + exp.ToString());
                ErrMsg = exp.ToString();
            }
            return null;
        }
        protected virtual void InitHttpRequest(HttpWebRequest httpRequest)
        {
            httpRequest.MediaType = "application/json";
            httpRequest.ContentType = "application/json;charset=utf-8";
            httpRequest.Headers.Add("user", this.UserName);
            httpRequest.Headers.Add("pwd", Md5.Encrypt(this.UserName + this.Pwd));
        }
    }
}