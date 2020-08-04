using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LiteCommerce.Customer.Startup))]
namespace LiteCommerce.Customer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
