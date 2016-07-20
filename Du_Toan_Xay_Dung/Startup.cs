using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Du_Toan_Xay_Dung.Startup))]
namespace Du_Toan_Xay_Dung
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
