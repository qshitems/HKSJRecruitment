using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Common.Mvc
{
    public class RightValidation
    {
        public const string AnonymousUrlApplicationKey = "AnonymousListSessionApplication";
        public const string RightsUrlSessionKey = "RightsListSession";
        private int _currentUserId;
        private System.Web.HttpContext _httpContext;

        public RightValidation()
        {
            _httpContext = System.Web.HttpContext.Current;
        }

        /// <summary>
        /// 有没有权限访问
        /// </summary>
        /// <returns></returns>
        public bool HasRight(string controller, string action)
        {
            int userId = System.Web.HttpContext.Current.AppUserId();
            if (userId <= 0)       //未登录
            {
                _message = "用户未登录!";
                return false;
            }
            this._currentUserId = userId;
            return HasRight(userId, controller, action);
        }
        /// <summary>
        /// 有没有权限访问
        /// </summary>
        /// <returns></returns>
        public bool HasRight(int userId, string controller, string action)
        {
            if (UserHelper.IgnoreCheckRight)
            {
                _message = "忽略权限验证";
                return true;
            }
            string url = GetUrl(controller, action);
            if (IsAnonymous(url))             //此处过滤匿名访问的页面
            {
                return true;
            }
            return HasRight(userId, url);
        }

        /// <summary>
        /// 有没有访问权限
        /// 实时查询
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="url">访问地址</param>
        /// <returns></returns>
        public bool HasRight(int userId, string url)
        {
            if (UserHelper.IgnoreCheckRight)
            {
                _message = "忽略权限验证";
                return true;
            }
            if (!RightsList.Exists(c => c.Equals(url, StringComparison.OrdinalIgnoreCase)))
            {
                _message = string.Format("权限不足:{0} userId:{1}", url.ToLower(), userId);
                return false;
            }
            _message = string.Format("可以访问:{0}", url);
            return true;
        }

        /// <summary>
        /// 是否匿名访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsAnonymous(string controller, string action)
        {
            string url = GetUrl(controller, action);
            return IsAnonymous(url);
        }

        /// <summary>
        /// 是否匿名访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsAnonymous(string url)
        {
            if (!AnonymousList.Exists(c => c.Equals(url, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            _message = string.Format("匿名访问:{0}", url.ToLower());
            return true;
        }

        /// <summary>
        /// 格式: /VirtualDirectory/Controller/Action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string GetUrl(string controller, string action)
        {
            HttpRequest Request = System.Web.HttpContext.Current.Request;
            string Host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            string AppRelativeCurrentExecutionFilePath = Request.AppRelativeCurrentExecutionFilePath;
            string CurrentExecutionFilePath = Request.CurrentExecutionFilePath;

            if (!string.IsNullOrEmpty(port))
            {
                port = ":" + port;
            }
            string virtualDirectory = string.Empty;
            string appStr = AppRelativeCurrentExecutionFilePath;
            if (appStr.StartsWith("~"))
            {
                appStr = appStr.Substring(appStr.IndexOf("~") + 1);
            }
            string curStr = CurrentExecutionFilePath;
            if (curStr.EndsWith(appStr))
            {
                int length = curStr.IndexOf(appStr);
                if (length > 0)
                    virtualDirectory = curStr.Substring(0, length);
            }
            return string.Format("{0}/{1}/{2}", virtualDirectory, controller, action);
        }

        /// <summary>
        /// 当前用户可以访问的权限数据
        /// </summary>
        private List<string> RightsList
        {
            get
            {
                if (_httpContext.Session[RightsUrlSessionKey] != null)        //不用重新加载
                {
                    return _httpContext.Session[RightsUrlSessionKey] as List<string>;
                }
                List<string> rightsList = LoadRightUrl();
                _httpContext.Session[RightsUrlSessionKey] = rightsList;
                return rightsList;
            }
        }

        private static List<string> _anonymousList;

        /// <summary>
        /// 匿名权限
        /// </summary>
        private List<string> AnonymousList
        {
            get
            {
                if (_anonymousList != null)
                    return _anonymousList;
                _anonymousList = LoadAnonymousUrl();
                return _anonymousList;
            }
        }
        /// <summary>
        /// 加载权限
        /// </summary>
        /// <returns></returns>
        private List<string> LoadRightUrl()
        {
            List<string> rightsList = new List<string>();
            Assembly assebly = AssemblyConfig.WebAssembly;
            Type type = assebly.GetTypes().FirstOrDefault(c => c.Name == "RightHeper");
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("LoadUserRight");
            rightsList = method.Invoke(obj, new object[] { this._currentUserId }) as List<string>;
            return rightsList;
        }

        /// <summary>
        /// 加载匿名权限
        /// </summary>
        /// <returns></returns>
        private List<string> LoadAnonymousUrl()
        {
            List<string> retList = new List<string>();
            if (AssemblyConfig.WebAssembly == null)
            {
                throw new ArgumentException("没有调用AssemblyConfig.InitWebAssembly,将影响权限验证!");
            }
            Type[] controllerSet = AssemblyConfig.WebAssembly.GetTypes().Where(t => t.Name.EndsWith("Controller")).ToArray();
            foreach (Type controller in controllerSet)
            {
                MethodInfo[] methodSet = controller.GetMethods();
                foreach (MethodInfo method in methodSet)
                {
                    if (!method.IsDefined(typeof(AnonymousAttribute), false))
                        continue;
                    //Controller 字符串长度为10
                    string url = GetUrl(controller.Name.Substring(0, controller.Name.Length - 10), method.Name);
                    if (retList.Exists(c => c.Equals(url, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    retList.Add(url);
                }
            }
            return retList;
        }

        protected string _message;
        /// <summary>
        /// 验证信息
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
    }
}