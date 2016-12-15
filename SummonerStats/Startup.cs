using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SummonerStats.Startup))]
namespace SummonerStats
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
