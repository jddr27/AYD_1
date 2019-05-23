using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carrito_Compras.Startup))]
namespace Carrito_Compras
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
