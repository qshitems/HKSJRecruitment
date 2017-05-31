using System.Web;
using System.Web.Routing;
using System.Linq;
using System;
using System.Reflection;

namespace Common.Mvc
{
    public static class UserHelper
    {
        /// <summary>
        /// 用户信息在Session里的key
        /// </summary>
        public static string SystemUserIdSessionKey = "SystemUserIdSessionKey";
        public static string SystemUserSessionKey = "SystemUserSessionKey";

        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static int AppUserId(this HttpContextBase httpContext)
        {
            if (httpContext.Session == null)
                throw new Exception("当前Session不可用!");
            if (httpContext.Session[UserHelper.SystemUserIdSessionKey] == null)
                return 0;
            return Convert.ToInt32(httpContext.Session[UserHelper.SystemUserIdSessionKey]);
        }
        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static int AppUserId(this HttpContext httpContext)
        {
            if (httpContext.Session == null)
                throw new Exception("当前Session不可用!");
            if (httpContext.Session[UserHelper.SystemUserIdSessionKey] == null)
                return 0;
            return Convert.ToInt32(httpContext.Session[UserHelper.SystemUserIdSessionKey]);
        }
        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string EmployeeName(this HttpContext httpContext)
        {
            if (httpContext.Session == null)
                throw new Exception("当前Session不可用!");
            if (httpContext.Session[UserHelper.SystemUserSessionKey] == null)
                return string.Empty;
            return httpContext.Session[UserHelper.SystemUserSessionKey].ToString();
        }
                /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string EmployeeName(this HttpContextBase httpContext)
        {
            if (httpContext.Session == null)
                throw new Exception("当前Session不可用!");
            if (httpContext.Session[UserHelper.SystemUserSessionKey] == null)
                return string.Empty;
            return httpContext.Session[UserHelper.SystemUserSessionKey].ToString();
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogined
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null)
                    throw new Exception("当前Session不可用!");
                return System.Web.HttpContext.Current.Session[SystemUserIdSessionKey] != null;
            }
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        public static void LoadDbUser()
        {
            LoadDbUser(DefaultAutoLoginUserId);
        }

        /// <summary>
        /// 根据nodeId装载用户信息
        /// </summary>
        /// <param name="nodeId"></param>
        public static void LoadDbUser(int userId)
        {
            Assembly assebly = AssemblyConfig.WebAssembly;
            Type type = assebly.GetTypes().FirstOrDefault(c => c.Name == "RightHeper");
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("HasUser");
            bool result = (bool)method.Invoke(obj, new object[] { userId });
            if (!result)
            {
                throw new Exception("用户不存在! userId=" + userId);
            }
            method = type.GetMethod("GetUserName");
            string userName = (string)method.Invoke(obj, new object[] { userId });
            Login(userId, userName);
        }
        public static void Login(int userId)
        {
            System.Web.HttpContext.Current.Session[UserHelper.SystemUserIdSessionKey] = userId;
        }
        public static void Login(int userId, string userName)
        {
            System.Web.HttpContext.Current.Session[UserHelper.SystemUserIdSessionKey] = userId;
            System.Web.HttpContext.Current.Session[UserHelper.SystemUserSessionKey] = userName;
        }
        public static void LogOff()
        {
            System.Web.HttpContext.Current.Session.Clear();
        }
        /// <summary>
        /// 调试模式 才能忽略页面权限检查，用于调试
        /// </summary>
        public static bool IgnoreCheckRight
        {
            get
            {
                return GetProgramTestConfigBoolValue(ProgramTestConfig.ssnIgnoreCheckRight);
            }
        }

        /// <summary>
        /// 调试下,自动登陆
        /// </summary>
        public static bool IsAutoLogin
        {
            get
            {
                return GetProgramTestConfigBoolValue(ProgramTestConfig.ssnIsAutoLogin);
            }
        }

        /// <summary>
        /// 自动登录用户ID
        /// </summary>
        private static int DefaultAutoLoginUserId
        {
            get
            {
                if (System.Web.HttpContext.Current.Application[ProgramTestConfig.ssnDefaultAutoLoginUserId] == null)
                {
                    ProgramTestConfig testConfig = new ProgramTestConfig(System.Web.HttpContext.Current.Request.PhysicalApplicationPath);
                    System.Web.HttpContext.Current.Application[ProgramTestConfig.ssnDefaultAutoLoginUserId] = testConfig.DefaultAutoLoginUserId;
                }
                return (int)System.Web.HttpContext.Current.Application[ProgramTestConfig.ssnDefaultAutoLoginUserId];
            }
        }

        /// <summary>
        /// 取ProgramTestConfigValue
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool GetProgramTestConfigBoolValue(string str)
        {
            if (System.Web.HttpContext.Current.Application[str] == null)
            {
                ProgramTestConfig testConfig = new ProgramTestConfig(System.Web.HttpContext.Current.Request.PhysicalApplicationPath);
                System.Web.HttpContext.Current.Application[str] = testConfig.GetConfigBoolValue(str);
            }
            return (bool)System.Web.HttpContext.Current.Application[str];
        }
    }
}