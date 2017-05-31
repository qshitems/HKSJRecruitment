using System.Reflection;

namespace Common.Mvc
{
    public class AssemblyConfig
    {
        /// <summary>
        /// 初始化Web程序集
        /// </summary>
        public static void InitWebAssembly()
        {
            WebAssembly = Assembly.GetCallingAssembly();
        }

        /// <summary>
        /// Web程序集
        /// 在Global.asax Application_Start中初始化
        /// </summary>
        public static Assembly WebAssembly { get; set; }
    }
}
