using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdChimeProject.Controllers
{
    public class AdminController : Controller
    {

        AdChimeProejctEntities dbadchime = new AdChimeProejctEntities();
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
            if (Session["idlogin"] != null)
            {
                var usersplataforma = dbadchime.tUsers.ToList();
                return View(usersplataforma);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult EditUser(int iduser)
        {
            var result = dbadchime.tUsers.Where(x => x.idUser == iduser).ToList();

            return View(result);
        }

        [HttpPost]
        public ActionResult EditUser(int id, string name, string email, string password, string isadmin)
        {
            tUser objc = dbadchime.tUsers.Single(x => x.idUser == id);
            objc.Name = name;
            objc.Email = email;
            if (isadmin != null)
            {
                objc.isadmin = true;
            }
            else
            {
                objc.isadmin = false;
            }


            if (password != null)
            {
                var newpasswordhash = GetStringSha256Hash(password);
                objc.Password = newpasswordhash;
            }
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
            if (Session["idlogin"] != null)
            {
                try
                {
                    var numerodemsgs = Int32.Parse(dbadchime.tSMSCounters.Where(x => x.idcounter == 1).Select(x => x.Counter).FirstOrDefault().ToString());
                    tSMSCounter objc = dbadchime.tSMSCounters.Single(ccc => ccc.idcounter == 1);
                    objc.Counter = numerodemsgs + Int32.Parse(nmbr);
                    Session["valuenow"] = (numerodemsgs + Int32.Parse(nmbr)).ToString();
                    Session["text"] = (numerodemsgs + Int32.Parse(nmbr)).ToString() + " available SMS's";
                    if ((numerodemsgs + Int32.Parse(nmbr)) / 5000 > 1)
                    {
                        Session["percentage"] = "100%";
                    }
                    else
                    {
                        Session["percentage"] = (((numerodemsgs + Int32.Parse(nmbr)) / 5000) * 100).ToString() + "%";
                    }

                }
                catch
                {
                    Session["valuenow"] = "5000";
                    Session["text"] = "ERROR GETTING THE MESSAGES";
                    Session["percentage"] = "100%";
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}