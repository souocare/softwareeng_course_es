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

        private string connectionstringgeral = "Server= localhost; Database= AdChimeProejct; Integrated Security=True;";
        AdChimeProejctEntities dbadchime = new AdChimeProejctEntities();
        // GET: Campaigns
        public ActionResult Campaigns(int? page)
        {
            ViewBag.Current = "Campaigns";
            return View(dbadchime.tCampaigns.OrderByDescending(x => x.updatedate).ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult NewCampaign()
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            var listofrecipients = dbadchime.tRecipientSms.ToList();
            foreach (var rec in listofrecipients)
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.country = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            ViewBag.Current = "Campaigns";
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewCampaign(tCampaign model)
        {

            using (SqlConnection connection = new SqlConnection(connectionstringgeral))
            {
                connection.Open();

                SqlCommand cmdaddlinepoints = new SqlCommand("INSERT INTO  [dbo].[tCampaign] ([TitleCampaing] ,[Description] ,[Sender] ,[Text] ,[idtemplate] ,[updatedbyuser]) VALUES (@param1,@param2,@param3, @param4, @param5, @param6)", connection);
                cmdaddlinepoints.Parameters.AddWithValue("@param1", model.TitleCampaing);
                cmdaddlinepoints.Parameters.AddWithValue("@param2", model.Description);
                cmdaddlinepoints.Parameters.AddWithValue("@param3", model.Sender);
                cmdaddlinepoints.Parameters.AddWithValue("@param4", model.Text);
                cmdaddlinepoints.Parameters.AddWithValue("@param5", model.idtemplate);
                cmdaddlinepoints.Parameters.AddWithValue("@param6", User.Identity.Name.ToString());
                cmdaddlinepoints.ExecuteNonQuery();

                connection.Close();
            };

            ViewBag.Current = "Campaigns";
            return View();
        }

        public ActionResult ChooseSMSText(int? page)
        {

            return View(dbadchime.tTemplateSms.Where(x => x.isaproved == true).OrderByDescending(x => x.updatedate).ToList().ToPagedList(page ?? 1, 20));
        }


        public ActionResult SendCampaign(int id)
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            var listofrecipients = dbadchime.tRecipientSms.ToList();
            foreach (var rec in listofrecipients)
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.listofrecipients = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            ViewBag.Current = "Campaigns";
            tCampaign tcamp = dbadchime.tCampaigns.Find(id);
            ViewBag.toolEmail = tcamp;
            return View();
        }

        [HttpPost]
        public ActionResult SendCampaign(tCampaignSend modelsend)
        {
            using (SqlConnection connection = new SqlConnection(connectionstringgeral))
            {
                connection.Open();

                SqlCommand cmdaddlinepoints = new SqlCommand("INSERT INTO  [dbo].[tCampaignSend] ([idcampaing]  ,[idrecipient]  ,[sDatetoSend], [sSendbyWho]) VALUES (@param1,@param2,@param3, @param4)", connection);
                cmdaddlinepoints.Parameters.AddWithValue("@param1", modelsend.idcampaing);
                cmdaddlinepoints.Parameters.AddWithValue("@param2", modelsend.idrecipient);
                cmdaddlinepoints.Parameters.AddWithValue("@param3", modelsend.sDatetoSend);
                cmdaddlinepoints.Parameters.AddWithValue("@param4", User.Identity.Name.ToString());
                cmdaddlinepoints.ExecuteNonQuery();

                connection.Close();
            };
            return RedirectToAction("Campaigns");
        }


        public ActionResult EditCampaign(int id)
        {
            Dictionary<string, string> trecipients = new Dictionary<string, string>();
            var listofrecipients = dbadchime.tRecipientSms.ToList();
            foreach (var rec in listofrecipients)
            {
                trecipients.Add(rec.idrecipient.ToString(), "#" + rec.idrecipient.ToString() + " - " + rec.TitleRecipient.ToString() + " - " + rec.NumberOfRecords.ToString() + " records ");
            }

            ViewBag.listofrecipients = new SelectList(trecipients.OrderBy(x => x.Key), "Key", "Value");

            ViewBag.Current = "Campaigns";
            return View(dbadchime.tCampaigns.Where(x => x.idcampaign == id).First());
        }

        [HttpPost]
        public ActionResult EditCampaign(tCampaign model)
        {
            using (SqlConnection connection = new SqlConnection(connectionstringgeral))
            {
                connection.Open();

                SqlCommand commandcheck = new SqlCommand("UPDATE [dbo].[tCampaign] SET [TitleCampaing] = @title, [Description] = @description, [Sender] = @sender, [Text] = @text, " +
                        " [idtemplate] = @idtemplate, [updatedate] = @updatedate,  " +
                        " [updatedbyuser] = @updatebyuser WHERE [idcampaign] = @idcampaign", connection);
                commandcheck.Parameters.AddWithValue("@title", model.TitleCampaing.ToString());
                commandcheck.Parameters.AddWithValue("@description", model.Description.ToString());
                commandcheck.Parameters.AddWithValue("@sender", model.Sender.ToString());
                commandcheck.Parameters.AddWithValue("@text", model.Text.ToString());
                commandcheck.Parameters.AddWithValue("@idtemplate", model.idtemplate);
                commandcheck.Parameters.AddWithValue("@updatedate", DateTime.Now);
                commandcheck.Parameters.AddWithValue("@updatebyuser", User.Identity.Name.ToString());
                commandcheck.Parameters.AddWithValue("@idcampaign", model.idcampaign);

                commandcheck.ExecuteNonQuery();
                connection.Close();

            };

            ViewBag.Current = "Campaigns";
            return RedirectToAction("Campaigns");
        }

        public ActionResult ChooseSMSTextEdit(int? page)
        {

            return View(dbadchime.tTemplateSms.Where(x => x.isaproved == true).OrderByDescending(x => x.updatedate).ToList().ToPagedList(page ?? 1, 20));
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

                        sLink newlink = new sLink
                        {
                            sOriginalLink = link,
                            sShorterLink = finallink
                        };
                        dbadchime.sLinks.Add(newlink);
                        dbadchime.SaveChanges();

                        var getidlink = dbadchime.sLinks.Where(x => x.sShorterLink == finallink).Select(x => x.idlink).FirstOrDefault().ToString();


                        return "{\"id\":\"" + getidlink + "\", \"linkfinal\":\"" + finallink + "\"}";
                    }
                }
            }

            return "{\"id\":\"" + "0" + "\", \"linkfinal\":\"" + "ERRO, TENTE NOVAMENTE" + "\"}";
            //return RedirectToAction(model.DesignId, "Prdocut/Edit");       
        }
    }
}