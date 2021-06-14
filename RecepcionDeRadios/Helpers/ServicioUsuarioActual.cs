using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Helpers
{
    public class ServicioUsuarioActual: IServicioUsuarioActual
    {
        private readonly IHttpContextAccessor httpContextAccesor;
        public ServicioUsuarioActual(IHttpContextAccessor httpContextAccessor) {

            this.httpContextAccesor = httpContextAccessor ?? throw new ArgumentNullException();
        }

    public string ObtenerNombreUsuarioActual()
        {
            return httpContextAccesor.HttpContext.User.Identity.Name;
        }
    }
}