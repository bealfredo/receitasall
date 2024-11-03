using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(receitasall.Startup))]
namespace receitasall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
