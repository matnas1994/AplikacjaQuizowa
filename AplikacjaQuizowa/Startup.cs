using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AplikacjaQuizowa.Startup))]
namespace AplikacjaQuizowa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
