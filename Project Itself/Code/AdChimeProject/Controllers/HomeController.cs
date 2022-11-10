using AdChimeProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdChimeProject.Controllers
{
    
    public class HomeController : Controller
    {

        AdChimeProejctEntities dbadchime = new AdChimeProejctEntities();
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var user_check = dbadchime.tUsers.Where(x => x.Email == model.Email).FirstOrDefault();
            if (user_check == null)
            {
                return RedirectToAction("Login", "Home");
            } else
            {
                if ( GetStringSha256Hash(model.Password) == model.Password)
                {
                    return RedirectToAction("MyContacts", "Contacts");
                } else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

        }
    }
}