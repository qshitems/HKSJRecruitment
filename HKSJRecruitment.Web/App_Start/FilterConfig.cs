using System.Web;
using System.Web.Mvc;

namespace HKSJRecruitment.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}