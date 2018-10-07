using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaypalIntegration.Startup))]
namespace PaypalIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
