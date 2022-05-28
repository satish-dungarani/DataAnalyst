using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using DataAnalystDB;
using Microsoft.AspNet.Identity;
using DataAnalyst.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace DataAnalyst.Controllers
{
    public class BaseController : Controller
    {
            public BaseController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {

        }
            public BaseController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                base.Initialize(requestContext);
                //clsCommonUI._Terminal = System.Net.Dns.GetHostName();

                //if (requestContext.RouteData.Values["action"].ToString().ToUpper() == "LOGIN" || requestContext.RouteData.Values["action"].ToString().ToUpper() == "LOGOFF")
                //{

                //    Session.Remove("UserName");
                //    return;
                //}
                //else
                //{
                //    if (Session == null || Session["UserName"] == null)
                //    {
                //        requestContext.HttpContext.Response.Close();
                //        requestContext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                //        requestContext.HttpContext.Response.End();
                //        //prequestcontext.HttpContext.Response.Clear();
                //        //prequestcontext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                //        //prequestcontext.HttpContext.Response.End();
                //        //filterContext.Result = new RedirectResult(Url.Action("Login", "Account"),true);
                //        //return;
                //        //RedirectToAction("Login", "Account");

                //    }
                //    else
                //    {
                //        if (Session != null || Session["AccessMenuList"] != null)
                //        {
                //            if (clsCommonUI.checkAccessAlloworNot((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"]) == false)
                //            {
                //                requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home"), false);
                //                //filterContext.Result = new RedirectResult(Url.Action("Index", "Home"), true);
                //                //return;
                //                //RedirectToAction("Index", "Home");
                //            }

                //        }
                //    }

                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                //System.Web.Routing.RequestContext prequestcontext = new RequestContext();


                clsCommonUI._Terminal = System.Net.Dns.GetHostName();

                if (filterContext.ActionDescriptor.ActionName.ToUpper() == "LOGIN" || filterContext.ActionDescriptor.ActionName.ToUpper() == "LOGOFF")
                {

                    //Session.Remove("UserName");
                    Session.RemoveAll();
                    //return;
                }
                else
                {
                    if (Session == null || Session["UserName"] == null)
                    {
                        filterContext.Result = new RedirectResult("/Account/Login");
                        return;
                        //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
                        //RedirectToAction("LogOff", "Account");
                        //prequestcontext.HttpContext.Response.Clear();
                        //prequestcontext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                        //filterContext.Result = RedirectToAction("Login", "Account");
                        //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "LogOff", controller = "Account" }));
                        //return;
                        //RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        if (Session != null || Session["AccessMenuList"] != null)
                        {
                            if (clsCommonUI.checkAccessAlloworNot((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"]) == false)
                            {
                                //prequestcontext.HttpContext.Response.Redirect(Url.Action("Index", "Home"), false);
                                //filterContext.Result = RedirectToAction("Index", "Home");
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
                                //filterContext.Result = new RedirectResult(Url.Action("Index", "Home"), true);
                                //return;
                                //RedirectToAction("Index", "Home");
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            base.OnActionExecuting(filterContext);
        }

        //
        // POST: /Home/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Manage(ManageUserViewModel model)
        {
            try
            {


                bool hasPassword = HasPassword();
                ViewBag.HasLocalPassword = hasPassword;
                ViewBag.ReturnUrl = Url.Action("Manage");
                if (hasPassword)
                {
                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            Session.RemoveAll();
                            return Json(new { Result = true, Message = "Password chabge successfully!" }, JsonRequestBehavior.AllowGet);
                            //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                        }
                        else
                        {
                            return Json(new { Result = false, Message = "Old Password is wrong!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    // User does not have a password so remove any validation errors caused by a missing OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }

                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                        if (result.Succeeded)
                        {
                            Session.RemoveAll();
                            return Json(new { Result = true, Message = "Password chabge successfully!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Result = false, Message = "Old Password is wrong!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { Result = false, Message = "Server Error. Try again later!" }, JsonRequestBehavior.AllowGet);
                // If we got this far, something failed, redisplay form

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            //Logging the Exception
            filterContext.ExceptionHandled = true;

            var Result = this.View("Error", new HandleErrorInfo(exception,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString()));

            filterContext.Result = Result;

        }


    }
}
