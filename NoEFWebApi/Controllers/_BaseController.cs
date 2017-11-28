using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace NoEFWebApi.Controllers
{
    public class BaseController : Controller
    {
        public Guid UserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return Guid.Parse(User.Identity.GetUserId());

                return Guid.Empty;
            }
        }
    }
}