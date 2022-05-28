using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DataAnalyst.Startup))]
namespace DataAnalyst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
