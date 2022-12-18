using Newtonsoft.Json.Linq;
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
    public class ContactsController : Controller
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


        public static string UpdateAndAdd(DataTable dtbl, string updateuser, string userconnected, Dictionary<string, string> dictfields)
        {
            List<string> listofmaintable = new List<string>();
            listofmaintable.Add("Name");
            listofmaintable.Add("LastName");
            listofmaintable.Add("PhoneNumber");
            listofmaintable.Add("CountryCodePhone");
            listofmaintable.Add("Country");

            AdChimeProejctEntities dbentdois = new AdChimeProejctEntities();
            foreach (DataRow _row in dtbl.Rows)
            {
                var checkifuserexists = dbentdois.panelContacts.Select(x => x.idContact).FirstOrDefault();
                using (SqlConnection connection = new SqlConnection("Server= localhost; Database= AdChimeProejct; Integrated Security=True;"))
                {
                    connection.Open();

                    if (checkifuserexists == 0)
                    {
                        SqlCommand cmdinsertuserpanel = new SqlCommand("INSERT INTO  [dbo].[panelContacts] ([Name] ,[LastName], [bActive], [PhoneNumber], [CountryCodePhone], [Country], [optinSMS],  [updatedbyuser] ) VALUES (@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8)", connection);
                        cmdinsertuserpanel.Parameters.AddWithValue("@param1", _row[dictfields["Name"]].ToString());
                        cmdinsertuserpanel.Parameters.AddWithValue("@param2", _row[dictfields["LastName"]].ToString());
                        cmdinsertuserpanel.Parameters.AddWithValue("@param3", true);
                        cmdinsertuserpanel.Parameters.AddWithValue("@param4", _row[dictfields["PhoneNumber"]].ToString());
                        cmdinsertuserpanel.Parameters.AddWithValue("@param5", _row[dictfields["CountryCodePhone"]].ToString());
                        cmdinsertuserpanel.Parameters.AddWithValue("@param7", _row[dictfields["Country"]].ToString());
                        cmdinsertuserpanel.Parameters.AddWithValue("@param8", 1);
                        cmdinsertuserpanel.Parameters.AddWithValue("@param10", userconnected);
                        cmdinsertuserpanel.ExecuteNonQuery();

                        foreach (string key in dictfields.Keys)
                        {
                            if (listofmaintable.Contains(key))
                            {
                                //
                            }
                            else
                            {
                                var getidofvar = dbentdois.tVarContacts.Where(x => x.VarName == key).Select(x => x.idVar).FirstOrDefault();
                                SqlCommand cmdinsertvalverticaltable = new SqlCommand("INSERT INTO  [dbo].[panelContactsVariables] ([idUser] ,[idVar], [sValue], [updatedbyuser] ) VALUES (@param1,@param2,@param3,@param4)", connection);
                                cmdinsertvalverticaltable.Parameters.AddWithValue("@param1", checkifuserexists);
                                cmdinsertvalverticaltable.Parameters.AddWithValue("@param2", getidofvar);
                                cmdinsertvalverticaltable.Parameters.AddWithValue("@param3", _row[dictfields[key]].ToString());
                                cmdinsertvalverticaltable.Parameters.AddWithValue("@param4", userconnected);
                                cmdinsertvalverticaltable.ExecuteNonQuery();
                            }
                        }

                    }
                    else
                    {
                        if (updateuser == "true")
                        {
                            foreach (string key in dictfields.Keys)
                            {
                                if (listofmaintable.Contains(key))
                                {
                                    SqlCommand commandcheck = new SqlCommand("UPDATE [dbo].[panelUsers] SET [" + key + "] = @key, " +
                            "  [updatedate] = @updatedate,  " +
                            " [updatedbyuser] = @updatebyuser WHERE [idUser] = @iduser", connection);
                                    commandcheck.Parameters.AddWithValue("@key", _row[dictfields[key]].ToString());
                                    commandcheck.Parameters.AddWithValue("@updatedate", DateTime.Now);
                                    commandcheck.Parameters.AddWithValue("@updatebyuser", userconnected);
                                    commandcheck.Parameters.AddWithValue("@iduser", checkifuserexists);

                                    commandcheck.ExecuteNonQuery();
                                }
                                else
                                {
                                    var getidofvar = dbentdois.tVarContacts.Where(x => x.VarName == key).Select(x => x.idVar).FirstOrDefault();
                                    SqlCommand commandcheck = new SqlCommand("UPDATE [dbo].[panelContactsVariables] SET [sValue] = @svalue,  " +
                            "  [updatedate] = @updatedate,  " +
                            " [updatedbyuser] = @updatebyuser WHERE [idUser] = @iduser and [idVar] = @idvar ", connection);
                                    commandcheck.Parameters.AddWithValue("@svalue", _row[dictfields[key]].ToString());
                                    commandcheck.Parameters.AddWithValue("@updatedate", DateTime.Now);
                                    commandcheck.Parameters.AddWithValue("@updatebyuser", userconnected);
                                    commandcheck.Parameters.AddWithValue("@iduser", checkifuserexists);
                                    commandcheck.Parameters.AddWithValue("@idvar", getidofvar);

                                    commandcheck.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    connection.Close();
                };
            }

            return "a";
        }





        AdChimeProejctEntities dbadchime = new AdChimeProejctEntities();
        // GET: Contacts
        public ActionResult MyContacts(int? page, string insertedcontacts, string message)
        {
            if (insertedcontacts != null)
            {
                ViewBag.insertedcontacts = insertedcontacts;
            }
            ViewBag.Current = "Contacts";


            List<string> listadecolunas = new List<string>();
            var colunasdecima = dbadchime.tVarContacts.Select(x => x.VarName).ToList();
            foreach (var col in colunasdecima)
            {
                listadecolunas.Add(col);
            }
            ViewBag.Colunas = listadecolunas;

            return View(dbadchime.panelContacts.Where(x => x.optinSMS == 1).ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult ImportContacts()
        {
            ViewBag.Current = "Contacts";

            List<List<string>> systemfields_importcontactmanual = new List<List<string>>();
            systemfields_importcontactmanual.Add(new List<string> { "Name", "text"});
            systemfields_importcontactmanual.Add(new List<string> { "LastName", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "CountryCodePhone", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "PhoneNumber", "number" });
            systemfields_importcontactmanual.Add(new List<string> { "Country", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "isActive", "checkbox" });
            systemfields_importcontactmanual.Add(new List<string> { "Optin SMS", "checkbox" });

            foreach (var field in dbadchime.tVarContacts.ToList())
            {
                systemfields_importcontactmanual.Add(new List<string> { field.VarName, field.colTypeType });
            }
            ViewBag.Systemfields = systemfields_importcontactmanual;

            return View();
        }


        // GET: Contacts
        public ActionResult DeleteContact(int idcontact)
        {
            try
            {
                panelContact contactusr = dbadchime.panelContacts
                             .Where(i => i.idContact == idcontact)
                             .Single();

                dbadchime.panelContacts.Remove(contactusr);

                dbadchime.panelContactsVariables.RemoveRange(dbadchime.panelContactsVariables.Where(c => c.idContact == idcontact));

                dbadchime.SaveChanges();

                return RedirectToAction("MyContacts", new { id = "Record deleted!" });
            }
            catch
            {

                return RedirectToAction("MyContacts", new { id = "Record not possible to delete. Please, try again later!" });
            }
            
        }


        public ActionResult DetailsContacts(int idcontact)
        {
            var result = dbadchime.panelContacts.Where(x => x.idContact == idcontact).ToList();
            ViewData["ContactMain"] = result;

            return View(result);
        }


        public ActionResult EditContacts(int idcontact)
        {
            var contact = dbadchime.panelContacts.Where(s => s.idContact == idcontact).FirstOrDefault();

            return View(contact);
        }

        [HttpPost]
        public ActionResult EditContacts(panelContact std)
        {
            //update student in DB using EntityFramework in real-life application

            //update list by removing old student and adding updated student for demo purpose
            var student = dbadchime.panelContacts.Where(s => s.idContact == std.idContact).FirstOrDefault();
            if (student != null)
            {
                student.Name = std.Name;
                student.LastName = std.LastName;
                student.Country = std.Country;
                student.CountryCodePhone = std.CountryCodePhone;
                student.PhoneNumber = std.PhoneNumber;
                student.bActive = std.bActive;
                student.optinSMS = std.optinSMS;
                dbadchime.SaveChanges();
            }

            return RedirectToAction("MyContacts", new { id = "Record updated!" });
        }



        public ActionResult AddNewField()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewField(string nameoffield, string fieldtype)
        {
            tVarContact t = new tVarContact
            {
                visible = true,
                VarName = nameoffield,
                colNumber = null,
                colTypeType = fieldtype,
                colTypeFilter = "multipleotpion",
            };

            dbadchime.tVarContacts.Add(t);
            dbadchime.SaveChanges();
            return RedirectToAction("MyContacts");
        }


        [HttpPost]
        public ActionResult ImportContactsFiles(HttpPostedFileBase fileee, string fielddelimiter, string typeofencoding, string updatedatausertext, string createrecipient_text, string createrecipient_bool)
        {
            Random rnd = new Random();
            int valor = rnd.Next(100000000, 999999999);
            var fileName = Path.GetFileName(fileee.FileName);
            var listafilename = fileName.Split('.');
            var filetypee = listafilename[listafilename.Count() - 1];

            if (fileee != null && fileee.ContentLength > 0)
            {
                string columns = "";
                var path = Path.Combine(Server.MapPath("~/Files/Contacts/"), valor.ToString() + fileName);
                fileee.SaveAs(path);

                if (filetypee == "xlsx" || filetypee == "xls")
                {
                    var xlsfile = getxlsData(Path.Combine(Server.MapPath("~/Files/Contacts/"), valor.ToString() + fileName), true);
                    foreach (var col in xlsfile.Columns)
                    {
                        columns = columns + col.ToString() + "##";
                    }

                }
                else if (filetypee == "csv" || filetypee == "txt")
                {
                    var xlsfile = ConvertCSVtoDataTable(Path.Combine(Server.MapPath("~/Files/Contacts/"), valor.ToString() + fileName), fielddelimiter);
                    foreach (var col in xlsfile.Columns)
                    {
                        columns = columns + col.ToString() + "##";
                    }
                }
                else
                {
                    columns = "##";
                }

                ViewBag.Current = "Contacts";

                if (createrecipient_bool == "false")
                {
                    createrecipient_text = "";
                }

                return RedirectToAction("ImportContactsMapping", new { columns = columns, filename = valor.ToString() + fileName, encodingtype = typeofencoding, updateuser = updatedatausertext, createrecipient = createrecipient_text });
            }

            else

            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ImportContactUnique(FormCollection form)
        {
            var fdfsdf = form[0];
            return View();
        }



        public ActionResult ImportContactsMapping(string columns, string filename, string delimitador, string encodingtype, string updateuser, string createrecipient)
        {

            List<string> systemfields = new List<string>();
            systemfields.Add("Name");
            systemfields.Add("LastName");
            systemfields.Add("CountryCodePhone");
            systemfields.Add("PhoneNumber");
            systemfields.Add("VeevaID");
            systemfields.Add("Country");
            foreach (var field in dbadchime.tVarContacts.Select(x => x.VarName).ToList())
            {
                systemfields.Add(field);
            }
            systemfields.Add("New Column");


            List<string> filefields = new List<string>();
            foreach (var field in columns.Split(new string[] { "##" }, StringSplitOptions.None))
            {
                if (field.Length > 0)
                {
                    filefields.Add(field);
                }

            }

            ViewBag.Systemfields = systemfields;
            ViewBag.FileFields = filefields;

            ViewBag.filename = filename;
            ViewBag.delimitador = delimitador;
            ViewBag.encodingtype = encodingtype;
            ViewBag.updateuser = updateuser;
            ViewBag.createrecipient = createrecipient;
            ViewBag.Current = "Contacts";

            return View();
        }

        [HttpPost]
        public ActionResult ImportContactsMappingFinal(string output, string filestring, string delimitador, string encodingtype, string updateuser, string createrecipient)
        {
            Dictionary<string, string> dictfields = new Dictionary<string, string>();
            var aaaa = output.Substring(1, output.Length - 2).Replace("},{", "}###{");
            var listaoutput = aaaa.Split(new string[] { "###" }, StringSplitOptions.None);
            foreach (string lis in listaoutput)
            {
                JObject json = JObject.Parse(lis);
                var filefield = json["filefield"].ToString();
                var systemfield = json["systemfield"].ToString();
                var typefield = json["typefield"].ToString();
                dictfields.Add(systemfield, filefield);

                if (systemfield == "New Column")
                {
                    var typefieldfinal = "";
                    if (typefield == "Numbers")
                    {
                        typefieldfinal = "integer";
                    }
                    else if (typefield == "Text")
                    {
                        typefieldfinal = "string";
                    }
                    else if (typefield == "Single Option")
                    {
                        typefieldfinal = "singleoption";
                    }
                    else if (typefield == "Multiple Option")
                    {
                        typefieldfinal = "multipleotpion";
                    }
                    else if (typefield == "Date")
                    {
                        typefieldfinal = "date";
                    }
                    else
                    {
                        typefieldfinal = "";
                    }

                    using (SqlConnection connection = new SqlConnection(connectionstringgeral))
                    {
                        connection.Open();

                        SqlCommand cmdaddlinepoints = new SqlCommand("INSERT INTO  [dbo].[tVarUsers] ([visible] ,[VarName] ,[colType] ) VALUES (@param1,@param2,@param3)", connection);
                        cmdaddlinepoints.Parameters.AddWithValue("@param1", true);
                        cmdaddlinepoints.Parameters.AddWithValue("@param2", filefield);
                        cmdaddlinepoints.Parameters.AddWithValue("@param3", typefieldfinal);
                        cmdaddlinepoints.ExecuteNonQuery();

                        connection.Close();
                    };
                }
            }

            var listafilename = filestring.Split('.');
            var filetypee = listafilename[listafilename.Count() - 1];
            var frasefinal = "";

            if (filetypee == "xlsx" || filetypee == "xls")
            {
                var xlsfile = getxlsData(Path.Combine(Server.MapPath("~/Files/Contacts/"), filestring), true);
                var resultadofinal = UpdateAndAdd(xlsfile, updateuser, User.Identity.Name, dictfields);

                if (createrecipient == "")
                {
                    return Json(Url.Action("MyContacts", "Home", new { insertedcontacts = "The new contacts were added!" }));
                }
                else
                {
                    if (dictfields.Keys.Contains("VeevaID"))
                    {
                        var getuniqueveevaids = (from r in xlsfile.AsEnumerable()
                                                 select r["idRepondent"]).Distinct().ToList();
                        foreach (var vvid in getuniqueveevaids)
                        {

                        }
                    }
                    else
                    {

                    }
                }

            }
            else if (filetypee == "csv" || filetypee == "txt")
            {
                var xlsfile = ConvertCSVtoDataTable(Path.Combine(Server.MapPath("~/Files/Contacts/"), filestring), delimitador);
                var resultadofinal = UpdateAndAdd(xlsfile, updateuser, User.Identity.Name, dictfields);
            }
            else
            {
                var xlsfile = "";
            }



            return Json(Url.Action("MyContacts", "Home", new { insertedcontacts = "The new contacts were added!" }));
        }



    }
}