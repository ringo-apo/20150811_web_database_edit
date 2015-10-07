using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcModel.Startup))]
namespace MvcModel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
