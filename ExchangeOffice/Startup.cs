using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExchangeOffice.Startup))]
namespace ExchangeOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
