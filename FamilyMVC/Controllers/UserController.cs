using FamilyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FamilyMVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        HttpHelper httpHelper = new HttpHelper();
        public ActionResult Login()
        {
            try
            {
                if(Session["UserName"] != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch(Exception ex)
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            try
            {
               // if (Session["UserName"] != null)
                {
                    string UserName = form["UserName"];
                    string Password = form["Password"];

                    string url = "api/User/Login?UserName=" + UserName + "&Password=" + Password;

                    var userdetails = httpHelper.GetCall<User>(url).Result;
                    if (userdetails != null)
                    {
                        Session["UserName"] = UserName;
                        Session["Email"] = userdetails.EmailId;
                        Session["UserId"] = userdetails.UserId;
                        Session["UserType"] = userdetails.UserType;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.LoginError = "Bad Credentials, Invalid username or password";
                        return View();
                    }
                }
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.UserType = "User";
                    httpHelper.PostData<User>(user, "api/User/PostUserInfo");
                    return RedirectToAction("Login");
                }
                else
                {
                    return View();
                }
                
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }
        public ActionResult SignOut()
        {
            try
            {
                Session.Clear();
                Session.RemoveAll();
                return RedirectToAction("Login");
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }
    }
}