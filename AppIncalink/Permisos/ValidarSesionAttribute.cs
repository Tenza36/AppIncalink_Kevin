using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AppIncalink.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var usuario = session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                context.Result = new RedirectResult("~/Acceso/Login");
            }

            base.OnActionExecuting(context);
        }
    }
}
