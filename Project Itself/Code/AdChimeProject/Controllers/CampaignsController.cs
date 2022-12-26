using AdChimeProject.Core;
using AdChimeProject.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace AdChimeProject.Controllers
{
    public class CampaignsController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public CampaignsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CampaignsController()
        {
            _unitOfWork = new UnitOfWork(new AdChimeContext());
        }

        public ActionResult Campaigns(int? page)
        {
            ViewBag.Current = "Campaigns";
            return View(_unitOfWork.Campaings.GetAllCampaings().ToPagedList(page ?? 1, 20));
        }

        public ActionResult NewCampaign()
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            var listofrecipients = _unitOfWork.RecipientsLists.GetRecipientsLists();
            foreach (var rec in listofrecipients)
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.country = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            ViewBag.Current = "Campaigns";
            return View();
        }

        [HttpPost]
        public ActionResult NewCampaign(Campaings model)
        {
            _unitOfWork.Campaings.Add(new Campaings
            {
                TitleCampaing = model.TitleCampaing,
                Description = model.Description,
                Sender = model.Sender,
                idtemplate = model.idtemplate,
                updatedbyuser = Session["Nome"].ToString()
            });

            _unitOfWork.Complete();


            ViewBag.Current = "Campaigns";
            return View();
        }

        public ActionResult ChooseSMSText(int? page)
        {
            return View(_unitOfWork.TemplateSMS.GetAllTemplates().ToPagedList(page ?? 1, 20));
        }


        public ActionResult SendCampaign(int id)
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            foreach (var rec in _unitOfWork.RecipientsLists.GetRecipientsLists())
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.listofrecipients = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            //ViewBag.Current = "Campaigns";
            //tCampaign tcamp = dbadchime.tCampaigns.Find(id);
            //ViewBag.toolEmail = tcamp;
            return View();
        }

        [HttpPost]
        public ActionResult SendCampaign(tCampaignSend modelsend)
        {
            _unitOfWork.CampaingSend.Add(new tCampaignSend
            {
                idcampaing = modelsend.idcampaing,
                idrecipient = modelsend.idrecipient,
                sDatetoSend = modelsend.sDatetoSend,
                sSendbyWho = Session["Nome"].ToString()
            });
            _unitOfWork.Complete();

            return RedirectToAction("Campaigns");
        }


        public ActionResult EditCampaign(int id)
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            var listofrecipients = _unitOfWork.RecipientsLists.GetRecipientsLists();
            foreach (var rec in listofrecipients)
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.listofrecipients = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            ViewBag.Current = "Campaigns";
            return View(_unitOfWork.Campaings.Get(id));
        }

        [HttpPost]
        public ActionResult EditCampaign(Campaings model)
        {
            var campaing = _unitOfWork.Campaings.SingleOrDefault(c => c.idcampaign == model.idcampaign);
            if (campaing != null)
            {
                campaing.TitleCampaing = model.TitleCampaing;
                campaing.Description = model.Description;
                campaing.Sender = model.Sender;
                campaing.Text = model.Text;
                campaing.idtemplate = model.idtemplate;
                campaing.updatedate = DateTime.Now;
                campaing.updatedbyuser = Session["Nome"].ToString();

                _unitOfWork.Complete();
            }

            ViewBag.Current = "Campaigns";
            return RedirectToAction("Campaigns");
        }

        public ActionResult ChooseSMSTextEdit(int? page)
        {

            return View(_unitOfWork.TemplateSMS.GetAllTemplates().ToPagedList(page ?? 1, 20));
        }


        [HttpPost]
        public string GenerateLink(string link)
        {
            string urlEncurtada = string.Empty;   // armazena a url encurtada
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-ssl.bitly.com/v4/shorten"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer AUTHCODE");

                    request.Content = new StringContent("{\n  \"long_url\": \"" + link + "\",\n  \"domain\": \"bit.ly\"\n}");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        var fdsfdsf = responseContent.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                        JObject s = JObject.Parse(fdsfdsf);
                        string finallink = (string)s["link"];

                        _unitOfWork.Links.Add(new sLink
                        {
                            sOriginalLink = link,
                            sShorterLink = finallink
                        });
                        _unitOfWork.Complete();


                        var getidlink = _unitOfWork.Links.GetID_ShorterLink(finallink);


                        return "{\"id\":\"" + getidlink + "\", \"linkfinal\":\"" + finallink + "\"}";
                    }
                }
            }

            return "{\"id\":\"" + "0" + "\", \"linkfinal\":\"" + "ERRO, TENTE NOVAMENTE" + "\"}";
            //return RedirectToAction(model.DesignId, "Prdocut/Edit");       
        }




    }
}