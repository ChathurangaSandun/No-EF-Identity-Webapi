using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Identity.Core;
using Microsoft.AspNet.Identity.Owin;
using NoEFWebApi.Models;

namespace NoEFWebApi.Controllers
{
    public class AccountController : ApiController
    {
        private IdentitySignInManager _signInManager;
        private IdentityUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(IdentityUserManager userManager, IdentitySignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IdentitySignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<IdentitySignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public IdentityUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [System.Web.Mvc.HttpPost]
        public async Task<string>  Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, Audit = new Audit(default(Guid)) };
                var result =  await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //sorry cannot sign in before approve
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"#\">here</a>");

                    return "send and ok!";
                }
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return "ok";
        }

    }
}