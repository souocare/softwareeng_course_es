using AdChimeProject.Core;
using AdChimeProject.Models;
using AdChimeProject.Persistence;
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
        protected readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HomeController()
        {
            _unitOfWork = new UnitOfWork(new AdChimeContext());
        }

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
            Session["percentage"] = "0%";
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login_model, string returnUrl)
        {
            var appuser_loggedin = _unitOfWork.AppUsers.GetUser(login_model.Email).FirstOrDefault();
            if (appuser_loggedin == null)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                if (GetStringSha256Hash(login_model.Password).ToString() == appuser_loggedin.Password.ToString())
                {
                    try
                    {
                        var smscounter = _unitOfWork.SMSCounter.GetSMSCounter().FirstOrDefault().Counter;
                        Session["valuenow"] = smscounter.ToString();
                        Session["text"] = smscounter.ToString() + " available SMS's";
                        if (smscounter / 5000 > 1)
                        {
                            Session["percentage"] = "100%";
                        }
                        else
                        {
                            Session["percentage"] = ((smscounter / 5000) * 100).ToString() + "%";
                        }
                    }
                    catch
                    {
                        Session["valuenow"] = "5000";
                        Session["text"] = "ERROR GETTING THE MESSAGES";
                        Session["percentage"] = "100%";
                    }

                    Session["idlogin"] = appuser_loggedin.iDLogin.ToString();
                    Session["email"] = appuser_loggedin.Email.ToString();
                    Session["isadmin"] = appuser_loggedin.isadmin.ToString();
                    Session["Nome"] = appuser_loggedin.Name.ToString();
                    return RedirectToAction("MyContacts", "Contacts");

                } else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }

            
        }



        
        public ActionResult AddCourse()
        {
            _unitOfWork.Contacts.Add(new Contacts
            {
                Name = "New Course at " + DateTime.Now.ToShortDateString()
            });

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}