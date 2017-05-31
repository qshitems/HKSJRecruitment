using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Common.Mvc
{
    public class MainRouteHandler : IRouteHandler
    {
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MainMvcHandler(requestContext);
        }
    }
    public class MainMvcHandler : MvcHandler
    {
        public MainMvcHandler(RequestContext requestContext)
            : base(requestContext)
        { }

        protected override System.IAsyncResult BeginProcessRequest(HttpContext httpContext, System.AsyncCallback callback, object state)
        {
            if (!UserHelper.IsLogined && UserHelper.IsAutoLogin)
                UserHelper.LoadDbUser();

            string controller = RequestContext.RouteData.Values["Controller"].ToString();
            string action = RequestContext.RouteData.Values["Action"].ToString();

            RightValidation rightValidation = new RightValidation();
            if (rightValidation.IsAnonymous(controller, action))
            {
                return base.BeginProcessRequest(httpContext, callback, state);
            }

            if (!UserHelper.IsLogined)
            {
                if (RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    RequestContext.HttpContext.Response.Write(JsonConvert.SerializeObject(new { result = false, msg = "未登录或者登录超时" }));
                }
                else
                {
                    RequestContext.HttpContext.Response.Redirect("/account/login");
                }
                //RequestContext.HttpContext.Response.Write("<script>parent.location.href = '/account/login';</script>");
                RequestContext.HttpContext.Response.End();
                return null;
            }

            if (!rightValidation.HasRight(controller, action))
            {
                if (RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    RequestContext.HttpContext.Response.Write(JsonConvert.SerializeObject(new { result = false, msg = rightValidation.Message }));
                }
                else
                {
                    RequestContext.HttpContext.Response.Write(rightValidation.Message);
                }
                RequestContext.HttpContext.Response.End();
                return null;
            }

            return base.BeginProcessRequest(httpContext, callback, state);
        }
    }
}