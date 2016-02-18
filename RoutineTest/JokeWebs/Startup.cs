using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JokeWebs.Startup))]
namespace JokeWebs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
