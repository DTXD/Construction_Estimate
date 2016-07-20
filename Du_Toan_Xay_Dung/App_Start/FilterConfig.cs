using System.Web;
using System.Web.Mvc;

namespace Du_Toan_Xay_Dung
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
