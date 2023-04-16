using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BVSneaker.Startup))]
namespace BVSneaker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
