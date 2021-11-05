using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Models
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class CustomAuthor:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            User user = (User)HttpContext.Current.Session["Account"];
            if (user.Account != null && user.Account.vaiTro == Roles)
                return true;                        
            return false;
        }
    }
}