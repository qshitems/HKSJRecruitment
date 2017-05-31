using Common.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKSJRecruitment.Web
{
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                HttpContext.Current.Response.Write("CustomExceptionAttribute.OnException.filterContext is null");
                throw new ArgumentException("CustomExceptionAttribute.OnException.filterContext is null");
            }
            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce == null)
                {
                    HttpContext.Current.Response.Write("CustomExceptionAttribute.OnException.filterContext.Exception is null");
                    throw new ArgumentException("CustomExceptionAttribute.OnException.filterContext.Exception is null");
                }
                if (httpExce.GetHttpCode() != 500)
                {
                    //为什么要特别强调500 因为MVC处理HttpException的时候，如果为500则会自动
                    //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                    return;
                }
            }
            string controller = filterContext.RouteData.Values["Controller"].ToString();
            string action = filterContext.RouteData.Values["Action"].ToString();
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                Exception exception = filterContext.Exception as Exception;
                if (exception != null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    {
                        filterContext.HttpContext.Response.Write(JsonConvert.SerializeObject(new { Result = false, Msg = "处理异常,原因:" + exception.Message }, Formatting.Indented, DefaultJsonSerializerSettings.Default));
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Write("处理异常,原因:" + exception.Message);
                    }
                    filterContext.HttpContext.Response.End();
                    //写入日志 记录
                    filterContext.ExceptionHandled = true;//设置异常已经处理
                }
            }
        }

        public System.Threading.Tasks.Task ExecuteExceptionFilterAsync(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}