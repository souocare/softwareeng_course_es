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

        private string connectionstringgeral = "Server= localhost; Database= AdChimeProejct; Integrated Security=True;";
        private System.Data.DataTable getxlsData(string filexls, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            System.Data.DataTable _dtreturn = new System.Data.DataTable();
            DataTable dtSchema;
            var Sheetnecessaria = "";
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filexls + ";Extended Properties='Excel 12.0;HDR=" + header + ";IMEX=1;';"))
            {
                conn.Open();
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                Sheetnecessaria = dtSchema.Rows[0].Field<string>("TABLE_NAME").ToString();
            }

            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filexls + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;';";

            var adapter = new OleDbDataAdapter("SELECT * FROM [" + Sheetnecessaria + "] ", connectionString);
            var ds = new DataSet();

            adapter.Fill(_dtreturn);
            DataView dv = _dtreturn.DefaultView;
            _dtreturn = dv.ToTable();


            return _dtreturn;
        }


        private static DataTable ConvertCSVtoDataTable(string strFilePath, string delimiter)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                char delvalue;
                if (delimiter == "Semicolon")
                {
                    delvalue = ';';
                }
                else if (delimiter == "Tab")
                {
                    delvalue = '\t';
                }
                else
                {
                    delvalue = ',';
                }
                string[] headers = sr.ReadLine().Split(delvalue);
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(delvalue);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }



        AdChimeProejctEntities dbadchime = new AdChimeProejctEntities();
        // GET: Contacts
        public ActionResult MyTexts()
        {
            if (Session["email"] != null)
            {
                ViewBag.Current = "MyTexts";

                var modelo = dbadchime.tTemplateSms.OrderBy(x => x.idtemplate).ToList();
                List<List<string>> list = new List<List<string>>();
                foreach (var model in modelo)
                {
                    List<string> listamodl = new List<string>();
                    listamodl.Add(model.idtemplate.ToString());
                    listamodl.Add(model.Title.ToString());

                    list.Add(listamodl);
                };
                ViewBag.Testee = list;

                var modelv2 = dbadchime.tTemplateSms.OrderByDescending(x => x.updatedate).ToList();
                List<List<string>> listv2 = new List<List<string>>();
                foreach (var model in modelv2)
                {
                    List<string> listamodl = new List<string>();
                    listamodl.Add(model.idtemplate.ToString());
                    listamodl.Add(model.Title.ToString());

                    listv2.Add(listamodl);
                };
                ViewBag.Testeev2 = listv2;

                return View();
            } else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        public ActionResult MyTextSelected(int id)
        {
            if (Session["email"] != null)
            {
                ViewBag.Current = "MyTexts";

                var modelo = dbadchime.tTemplateSms.OrderBy(x => x.idtemplate).ToList();
                List<List<string>> list = new List<List<string>>();
                foreach (var model in modelo)
                {
                    List<string> listamodl = new List<string>();
                    listamodl.Add(model.idtemplate.ToString());
                    listamodl.Add(model.Title.ToString());

                    list.Add(listamodl);
                };
                ViewBag.Testee = list;

                var modelv2 = dbadchime.tTemplateSms.OrderByDescending(x => x.updatedate).ToList();
                List<List<string>> listv2 = new List<List<string>>();
                foreach (var model in modelv2)
                {
                    List<string> listamodl = new List<string>();
                    listamodl.Add(model.idtemplate.ToString());
                    listamodl.Add(model.Title.ToString());

                    listv2.Add(listamodl);
                };
                ViewBag.Testeev2 = listv2;

                var modelselected = dbadchime.tTemplateSms.Where(x => x.idtemplate == id).First();


                return View(modelselected);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }


            
        }

        [HttpPost]
        public ActionResult MyTextSelected(tTemplateSm modeltemplatesms)
        {
            ViewBag.Current = "MyTexts";
            using (SqlConnection connection = new SqlConnection(connectionstringgeral))
            {
                connection.Open();

                SqlCommand commandcheck = new SqlCommand("UPDATE [dbo].[tTemplateSms] SET [Title] = @title, [Text] = @text, " +
                        " [isaproved] = @isaproved,  [updatedate] = @updatedate,  " +
                        " [updatedbyuser] = @updatebyuser WHERE [idtemplate] = @idtemplate", connection);
                commandcheck.Parameters.AddWithValue("@title", modeltemplatesms.Title.ToString());
                commandcheck.Parameters.AddWithValue("@text", modeltemplatesms.Text.ToString());
                commandcheck.Parameters.AddWithValue("@isaproved", modeltemplatesms.isaproved);
                commandcheck.Parameters.AddWithValue("@updatedate", DateTime.Now);
                commandcheck.Parameters.AddWithValue("@updatebyuser", User.Identity.Name.ToString());
                commandcheck.Parameters.AddWithValue("@idtemplate", modeltemplatesms.idtemplate.ToString());

                commandcheck.ExecuteNonQuery();
                connection.Close();

            };

            var modelo = dbadchime.tTemplateSms.OrderBy(x => x.idtemplate).ToList();
            List<List<string>> list = new List<List<string>>();
            foreach (var model in modelo)
            {
                List<string> listamodl = new List<string>();
                listamodl.Add(model.idtemplate.ToString());
                listamodl.Add(model.Title.ToString());

                list.Add(listamodl);
            };
            ViewBag.Testee = list;

            var modelv2 = dbadchime.tTemplateSms.OrderByDescending(x => x.updatedate).ToList();
            List<List<string>> listv2 = new List<List<string>>();
            foreach (var model in modelv2)
            {
                List<string> listamodl = new List<string>();
                listamodl.Add(model.idtemplate.ToString());
                listamodl.Add(model.Title.ToString());

                listv2.Add(listamodl);
            };
            ViewBag.Testeev2 = listv2;


            var modelselected = dbadchime.tTemplateSms.Where(x => x.idtemplate == modeltemplatesms.idtemplate).First();
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
                } else
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