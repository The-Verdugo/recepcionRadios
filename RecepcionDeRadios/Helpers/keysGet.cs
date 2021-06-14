
using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Helpers
{
    public class keysGet
    {
        public string keygeyuser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return HttpContext.Current.User.Identity.Name;
            }
            else {
                return null;
            }
            
        }
    }
}