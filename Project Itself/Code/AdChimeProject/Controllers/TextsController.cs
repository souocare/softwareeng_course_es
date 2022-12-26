using AdChimeProject.Core;
using AdChimeProject.Persistence;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdChimeProject.Controllers
{
    public class TextsController : Controller
    {

        protected readonly IUnitOfWork _unitOfWork;
        public TextsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TextsController()
        {
            _unitOfWork = new UnitOfWork(new AdChimeContext());
        }


        public ActionResult MyTexts()
        {
            if (Session["email"] != null)
            {
                var all_templatesSMs = _unitOfWork.TemplateSMS.GetAllTemplates();
                return View(all_templatesSMs);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult MyTextSelected(int id)
        {
            if (Session["email"] != null)
            {
                ViewBag.Current = "MyTexts";

                var all_templatesSMs = _unitOfWork.TemplateSMS.GetAllTemplates();
                List<List<string>> all_templatesSMs_list = new List<List<string>>();
                foreach (var template in all_templatesSMs)
                {
                    List<string> lista_template = new List<string>();
                    lista_template.Add(template.idtemplate.ToString());
                    lista_template.Add(template.Title.ToString());

                    all_templatesSMs_list.Add(lista_template);
                };
                ViewBag.AllTemplates = all_templatesSMs_list;


                var modelselected = _unitOfWork.TemplateSMS.GetTemplateInfo(id);


                return View(modelselected);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpPost]
        public ActionResult MyTextSelected(TemplateSMS modeltemplatesms)
        {
            ViewBag.Current = "MyTexts";

            _unitOfWork.TemplateSMS.Add(new TemplateSMS
            {
                Title = modeltemplatesms.Title,
                Text = modeltemplatesms.Text,

                isaproved = modeltemplatesms.isaproved,
                updatedate = modeltemplatesms.updatedate,
                updatedbyuser = Session["Nome"].ToString(),
                idtemplate = modeltemplatesms.idtemplate,
            });

            _unitOfWork.Complete();
            

            var all_templatesSMs = _unitOfWork.TemplateSMS.GetAllTemplates();
            List<List<string>> all_templatesSMs_list = new List<List<string>>();
            foreach (var template in all_templatesSMs)
            {
                List<string> lista_template = new List<string>();
                lista_template.Add(template.idtemplate.ToString());
                lista_template.Add(template.Title.ToString());

                all_templatesSMs_list.Add(lista_template);
            };
            ViewBag.AllTemplates = all_templatesSMs_list;


            var modelselected = _unitOfWork.TemplateSMS.GetTemplateInfo(modeltemplatesms.idtemplate);


            return View(modelselected);
        }


        public ActionResult NewText()
        {
            if (Session["email"] != null)
            {
                if (Session["isadmin"].ToString() == "True")
                {
                    ViewBag.Current = "MyTexts";
                    return View();
                }
                else
                {
                    return RedirectToAction("MyTexts");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public ActionResult NewText(HttpPostedFileBase fileee, string irecipientetexto)
        {
            ViewBag.Current = "MyTexts";

            return View();
        }


    }
}