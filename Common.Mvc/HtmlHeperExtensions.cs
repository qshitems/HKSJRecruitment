using System.Web.Routing;
using Common.Mvc;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 有没有权限
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool HasRight(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            RightValidation right = new RightValidation();
            return right.HasRight(controllerName, actionName);
        }
    }
}
