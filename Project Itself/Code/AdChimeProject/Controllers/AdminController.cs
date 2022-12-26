using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdChimeProject.Controllers
{
    public class AdminController : Controller
    {
        
        // GET: Admin

        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public ActionResult AdminPage()
        {
            if (Session["idlogin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ManageLogin()
        {
            return View();
        }

        public ActionResult EditUser(int iduser)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(int id, string name, string email, string password, string isadmin)
        {
            return View("ManageLogin");
        }


        public ActionResult AddUser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddUser(string name, string email, string password, string isadmin)
        {
            return View("ManageLogin");
        }



        public ActionResult LoadSMSBalance()
        {
            if (Session["idlogin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult AddSMS(string nmbr)
        {
            return View();
        }
    }
}