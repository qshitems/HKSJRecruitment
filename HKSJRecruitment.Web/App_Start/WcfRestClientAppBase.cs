using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Common.Mvc;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace HKSJRecruitment.Web
{
    public class WcfRestClientAppBase : WcfRestClient
    {
        public WcfRestClientAppBase()
            : base()
        {
            BaseUrl = ConfigurationManager.AppSettings["WcfRestClient.AppBase.BaseUrl"];
            UserName = ConfigurationManager.AppSettings["WcfRestClient.UserName"];
            Pwd = ConfigurationManager.AppSettings["WcfRestClient.Pwd"];
        }
        public WcfRestClientAppBase(string prefix)
        {
            BaseUrl = Path.Combine(ConfigurationManager.AppSettings["WcfRestClient.AppBase.BaseUrl"], prefix);
            UserName = ConfigurationManager.AppSettings["WcfRestClient.UserName"];
            Pwd = ConfigurationManager.AppSettings["WcfRestClient.Pwd"];
        }
        public WcfRestClientAppBase(string baseUrl, string userName, string pwd)
            : base(baseUrl, userName, pwd)
        {

        }
        protected override void InitHttpRequest(HttpWebRequest httpRequest)
        {
            base.InitHttpRequest(httpRequest);
        }
    }
}