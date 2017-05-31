using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Mvc;
using HKSJRecruitment.Models.Models;
using Newtonsoft.Json;
using System.Reflection;
using HKSJRecruitment.Web.Models;

namespace HKSJRecruitment.Web
{
    public static class ControllerHelper
    {
        /// <summary>
        /// 获取默认权限按钮
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static List<Tapp_Button> LoadButton(this Controller controller)
        {
            int userId = HttpContext.Current.AppUserId();
            RightHeper rightHelper = new RightHeper();
            return rightHelper.LoadButton(userId, controller.RouteData.GetRequiredString("Controller"), controller.RouteData.GetRequiredString("Action"));
        }
        /// <summary>
        /// 获取权限按钮
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public static List<Tapp_Button> LoadButton(this Controller controller, string menuCode)
        {
            int userId = HttpContext.Current.AppUserId();
            RightHeper rightHelper = new RightHeper();
            return rightHelper.LoadButton(userId, menuCode);
        }
        /// <summary>
        /// 获取权限按钮
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static List<Tapp_Button> LoadButton(this Controller controller, int menuId)
        {
            int userId = HttpContext.Current.AppUserId();
            RightHeper rightHelper = new RightHeper();
            return rightHelper.LoadButton(userId, menuId);
        }
        /// <summary>
        /// 返回Json字符串[用Newtonsoft.Json 格式化时间yyyy-MM-dd HH:mm:ss，键统一小写,忽略循环引用]
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJSON(this Controller controller, object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, DefaultJsonSerializerSettings.Default);
        }
        /// <summary>
        /// 返回Json Content-Type
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string JsonContentType(this Controller controller)
        {
            //return "text/html";
            //return "application/Json";
            return "text/plain;";
        }

        /// <summary>
        /// 编辑Model时动态赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T CopyObject<T>(this Controller controller, T dest, T src)
        {
            string[] excepts = new string[] { "Id", "CreateBy", "CreateTime", "EditBy", "EditTime", "IsDelete", "DeleteBy", "DeleteTime" };
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Type propertyType = property.PropertyType;
                string propertyTypeName = propertyType.Name.ToLower();
                if ((propertyType.IsValueType || propertyTypeName == "string")
                    && (excepts.Count(c => c.Equals(property.Name, StringComparison.OrdinalIgnoreCase)) == 0))
                {
                    object value = property.GetValue(src, null);
                    if (value != null || (propertyTypeName == "string"))
                    {
                        property.SetValue(dest, value, null);
                    }
                }
            }
            return dest;
        }

        /// <summary>
        /// 获取Tapp_User
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static Tapp_User AppUser(this Controller controller)
        {
            HKSJRecruitmentContext ctx = new HKSJRecruitmentContext();
            int userId = HttpContext.Current.AppUserId();
            Tapp_User u = ctx.Tapp_User.FirstOrDefault(c => c.Id == userId);
            if (u != null)
            {
                u.UserPwd = string.Empty;
                u.Tapp_User_Role = null;
                //HttpContext.Current.Session[UserHelper.SystemUserSessionKey] = u;
            }            
            return u;
            //if (HttpContext.Current.Session[UserHelper.SystemUserSessionKey] == null)
            //{

            //}
            //return HttpContext.Current.Session[UserHelper.SystemUserSessionKey] as Tapp_User;
        }
    
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <returns></returns>
        public static List<UserRoleDto> LoadUserRole(this Controller controller)
        {
            RightHeper rightHelper = new RightHeper();
            return rightHelper.LoadUserRole();
        }

    }
}