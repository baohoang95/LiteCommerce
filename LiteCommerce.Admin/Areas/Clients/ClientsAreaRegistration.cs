using System.Web.Mvc;

namespace LiteCommerce.Admin.Areas.Clients
{
    public class ClientsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Clients";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Clients_default",
                "Clients/{controller}/{action}/{id}",
                new { action = "Index",controller="Home", id = UrlParameter.Optional }
            );
        }
    }
}