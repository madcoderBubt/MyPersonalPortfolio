using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPortfolio.Startup))]
namespace MyPortfolio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
