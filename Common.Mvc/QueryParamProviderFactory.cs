using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common.Mvc
{
    public class QueryParamProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            NameValueCollection nameValues = new NameValueCollection();
            IEnumerable<string> keys = controllerContext.HttpContext.Request.QueryString.AllKeys.Where(c => c.StartsWith("query", StringComparison.OrdinalIgnoreCase))
                .Union(controllerContext.HttpContext.Request.Form.AllKeys.Where(c => c.StartsWith("query", StringComparison.OrdinalIgnoreCase)));
            foreach (var key in keys)
            {
                nameValues.Add(key.Substring(5), controllerContext.HttpContext.Request[key]);
            }
            if (controllerContext.HttpContext.Request.QueryString.AllKeys.Count(c => c.Equals("rows", StringComparison.OrdinalIgnoreCase)) > 0
                || controllerContext.HttpContext.Request.Form.AllKeys.Count(c => c.Equals("rows", StringComparison.OrdinalIgnoreCase)) > 0)
            {
                nameValues.Add("PageSize", controllerContext.HttpContext.Request["rows"]);
            }
            if (controllerContext.HttpContext.Request.QueryString.AllKeys.Count(c => c.Equals("page", StringComparison.OrdinalIgnoreCase)) > 0
                 || controllerContext.HttpContext.Request.Form.AllKeys.Count(c => c.Equals("page", StringComparison.OrdinalIgnoreCase)) > 0)
            {
                nameValues.Add("PageIndex", controllerContext.HttpContext.Request["page"]);
            }
            return new NameValueCollectionValueProvider(nameValues, CultureInfo.CurrentCulture);
        }
    }
}