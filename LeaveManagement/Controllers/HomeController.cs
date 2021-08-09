using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessModel;
using DataModel.Contracts;
using DataModel.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "LeaveRequest");
        }

        [HttpGet]
        public ActionResult Login(string returnURL)
        {
            var userinfo = new LoginModel();
            try
            {
                // Ensure that any existing identity/authenticated request to be logged out    
                EnsureLoggedOut();
                // Store the originating URL that can be attached to a hidden form field    
                userinfo.ReturnURL = returnURL;
                return View(userinfo);
            }
            catch
            {
                throw;
            }
        }

        //GET: EnsureLoggedOut    
        private void EnsureLoggedOut()
        {
            // If the request is still authenticated, then perform the logout action for the User   
            if (Request.IsAuthenticated)
                Logout();
        }

        //POST: Logout    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
                // First we clean the authentication ticket like always    
                //required NameSpace: using System.Web.Security;    
                FormsAuthentication.SignOut();

                // Second we clear the principal to ensure the user does not retain any authentication    
                //required NameSpace: using System.Security.Principal;    
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place    
                // this clears the Request.IsAuthenticated flag since this triggers a new request    
                return RedirectToLocal();
            }
            catch
            {
                throw;
            }
        }

        //GET: SignInAsync       
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data    
            FormsAuthentication.SignOut();

            // Write the authentication cookie    
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        //GET: RedirectToLocal    
        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                // If the return url starts with a slash "/" we assume it belongs to our site    
                // so we will redirect to this "action"    
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return Redirect(returnURL);

                // If we cannot verify if the url is local to our host we redirect to a default location    
                return RedirectToAction("Index", "Dashboard");
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel entity)
        {
            try
            {
                // Ensure we have a valid viewModel to work with    
                if (!ModelState.IsValid)
                    return View(entity);

                //Convert plain Password to HASH Value using SHA512.
                // Assign the hashed value to PasswordHash of LoginModel and make the Password value as empty
                byte[] plainPasswordBytes = Encoding.UTF8.GetBytes(entity.Password);
                HashAlgorithm hash = new SHA512Managed();
                byte[] hashPasswordBytes = hash.ComputeHash(plainPasswordBytes);
                entity.PasswordHash = Convert.ToBase64String(hash.ComputeHash(plainPasswordBytes));

                entity.Password = string.Empty;

                var userService = new UserService();
                UserModel userDetails = await userService.IsValidUser(entity);
                if (userDetails != null)
                {
                    //Login Success    
                    //For Set Authentication in Cookie (Remeber ME Option)    
                    SignInRemember(userDetails.Email, true);
                    //Set A Unique ID in session    
                    Session["UserId"] = userDetails.Id;
                    Session["UserName"] = userDetails.Email;
                    Session["UserRole"] = userDetails.AccessType.Name;
                    if (userDetails.AccessTypeId.Equals(AccessTypeEnum.Admin))
                    {
                        return RedirectToAction("Index", "LeaveRequest");
                    }
                    else
                    {
                        return RedirectToAction("Index", "LeaveRequest");
                    }
                    // If we got this far, something failed, redisplay form    
                    // return RedirectToAction("Index", "Dashboard");    
                    //return RedirectToLocal(entity.ReturnURL);
                }
                else
                {
                    //Login Fail    
                    TempData["ErrorMSG"] = "Access Denied! Wrong Credential";
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}