using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdChimeProject.Controllers
{
    public class ListsController : Controller
    {

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
        public ActionResult MyLists(int? page, string errorname)
        {
            if (errorname == "sim")
            {
                ViewBag.MsgErroTitle = "sim";
            }
            ViewBag.Current = "MyLists";

            return View(dbadchime.tRecipientSms.OrderByDescending(x => x.updatedate).ToList().ToPagedList(page ?? 1, 20));
        }


        public ActionResult NewList(int? error)
        {
            if (error == 1)
            {
                ViewBag.MsgErroTitle = "sim";
            }
            

            var listadados = dbadchime.panelContacts.Where(x => x.optinSMS == 1);
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

            ViewBag.Systemfields = systemfields;

            ViewBag.Countries = listadados.Select(x => x.Country).Distinct().ToList();
            ViewBag.Nomes = listadados.Select(x => x.Name).Distinct().ToList();



            ViewBag.Current = "MyLists";
            return View();
        }



        [HttpPost]
        public ActionResult NewList(string irecipientetexto)
        {
            var checkIfAlreadyExists = dbadchime.tRecipientSms.Where(x => x.TitleRecipient == irecipientetexto).ToList();
            if (checkIfAlreadyExists.Count > 0)
            {
                return RedirectToAction("MyLists", new { errorname = "sim" });
            }


            ViewBag.Current = "MyLists";

            return RedirectToAction("MyLists");
        }


        public string Addlog(string elementvalue, string idv)
        {

            var listadados = dbadchime.panelContacts.Where(x => x.optinSMS == 1);


            var getcoltype = dbadchime.tVarContacts.Where(x => x.VarName == elementvalue).Select(x => x.colTypeFilter).FirstOrDefault();
            var hmtltext = "";
            if (getcoltype == null)
            {
                if (elementvalue == "Name" || elementvalue == "LastName" || elementvalue == "CountryCodePhone")
                {
                    hmtltext = "<input id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" type=\"text\" style=\"width: 100%; height: 100%; \" class=\"form-control\" />";
                }
                else if (elementvalue == "VeevaID")
                {
                    hmtltext = "<input id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" type=\"number\" style=\"width: 100%; height: 100%; \" class=\"form-control\" />";
                }
                else if (elementvalue == "Country")
                {
                    hmtltext = "<select id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" style=\"width: 100%; \" class=\"form-control select2-multiplethird_mudar\" multiple > ";
                    foreach (var country in listadados.Select(x => x.Country).Distinct().ToList())
                    {
                        hmtltext = hmtltext + " <option value=\"" + country + "\">" + country + "</option> ";
                    }
                    hmtltext = hmtltext + " </select> ";
                }
            }
            else
            {
                if (getcoltype == "integer")
                {
                    hmtltext = "<input id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" type=\"number\" style=\"width: 100%; height: 100%; \" class=\"form-control\" />";
                }
                else if (getcoltype == "string")
                {
                    hmtltext = "<input id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" type=\"text\" style=\"width: 100%; height: 100%; \" class=\"form-control\" />";
                }
                else if (getcoltype == "singleoption")
                {
                    hmtltext = "<select id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" style=\"width: 100%; \" class=\"form-control select2-multiplethird_mudar\"  > ";
                    var listadadospaneluser = dbadchime.panelContactsVariables.Where(x => x.panelContact.optinSMS == 1).Where(x => x.tVarContact.VarName == elementvalue).Select(x => x.sValue).Distinct().ToList();
                    foreach (var lis in listadadospaneluser)
                    {
                        hmtltext = hmtltext + " <option value=\"" + lis + "\">" + lis + "</option> ";
                    }
                    hmtltext = hmtltext + " </select> ";
                }
                else if (getcoltype == "multipleotpion")
                {
                    hmtltext = "<select id=\"systemfields_" + idv.ToString() + "_3\" name=\"systemfields_" + idv.ToString() + "_3\" style=\"width: 100%; \" class=\"form-control select2-multiplethird_mudar\" multiple > ";
                    var listadadospaneluser = dbadchime.panelContactsVariables.Where(x => x.panelContact.optinSMS == 1).Where(x => x.tVarContact.VarName == elementvalue).Select(x => x.sValue).Distinct().ToList();
                    foreach (var lis in listadadospaneluser)
                    {
                        hmtltext = hmtltext + " <option value=\"" + lis + "\">" + lis + "</option> ";
                    }
                    hmtltext = hmtltext + " </select> ";

                }
                else if (getcoltype == "date")
                {
                    hmtltext = "";
                }
                else
                {
                    hmtltext = "";
                }
            }

            return hmtltext;

        }




    }
}