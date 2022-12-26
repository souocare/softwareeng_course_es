using AdChimeProject.Core;
using AdChimeProject.Persistence;
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
        protected readonly IUnitOfWork _unitOfWork;

        public ContactsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ContactsController()
        {
            _unitOfWork = new UnitOfWork(new AdChimeContext());
        }

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


        public ActionResult MyContacts(int? page, string insertedcontacts, string message)
        {
            if (insertedcontacts != null)
            {
                ViewBag.insertedcontacts = insertedcontacts;
            }
            ViewBag.Current = "Contacts";


            List<string> lista_variaveis_opcionais = new List<string>();
            foreach (var col in _unitOfWork.VarContacts.GetAllVariableNames())
            {
                lista_variaveis_opcionais.Add(col);
            }
            ViewBag.Colunas = lista_variaveis_opcionais;

            return View(_unitOfWork.Contacts.GetContactsWithOptin().ToPagedList(page ?? 1, 20));
        }


        public ActionResult ImportContacts()
        {
            ViewBag.Current = "Contacts";

            List<List<string>> systemfields_importcontactmanual = new List<List<string>>();
            systemfields_importcontactmanual.Add(new List<string> { "Name", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "LastName", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "CountryCodePhone", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "PhoneNumber", "number" });
            systemfields_importcontactmanual.Add(new List<string> { "Country", "text" });
            systemfields_importcontactmanual.Add(new List<string> { "isActive", "checkbox" });
            systemfields_importcontactmanual.Add(new List<string> { "Optin SMS", "checkbox" });

            foreach (var field in _unitOfWork.VarContacts.GetAll())
            {
                systemfields_importcontactmanual.Add(new List<string> { field.VarName, field.colTypeType });
            }
            ViewBag.Systemfields = systemfields_importcontactmanual;

            return View();
        }


        public ActionResult DeleteContact(int idcontact)
        {
            try
            {
                var contacto = _unitOfWork.Contacts.Get(idcontact);
                var variables_of_contact = _unitOfWork.ContactsVariables.GetAllVariablesOfCertainContact(idcontact);
                _unitOfWork.ContactsVariables.RemoveRange(variables_of_contact); 
                _unitOfWork.Contacts.Remove(contacto);

                return RedirectToAction("MyContacts", new { id = "Record deleted!" });
            }
            catch
            {
                return RedirectToAction("MyContacts", new { id = "Record not possible to delete. Please, try again later!" });
            }
        }

        public ActionResult DetailsContacts(int idcontact)
        {
            var details_contact = _unitOfWork.Contacts.GetInfoContact(idcontact);

            return View(details_contact);
        }

        public ActionResult EditContacts(int idcontact)
        {
            var contact = _unitOfWork.Contacts.GetInfoContactEdit(idcontact);

            return View(contact);
        }

        [HttpPost]
        public ActionResult EditContacts(Contacts contact_form)
        {
            //update student in DB using EntityFramework in real-life application

            //update list by removing old student and adding updated student for demo purpose
            var contact = _unitOfWork.Contacts.SingleOrDefault(c => c.idContact == contact_form.idContact);
            if (contact != null)
            {
                contact.Name = contact_form.Name;
                contact.LastName = contact_form.LastName;
                contact.Country = contact_form.Country;
                contact.CountryCodePhone = contact_form.CountryCodePhone;
                contact.PhoneNumber = contact_form.PhoneNumber;
                contact.bActive = contact_form.bActive;
                contact.optinSMS = contact_form.optinSMS;

                _unitOfWork.Complete();
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
            var varcontact = new tVarContact();

            _unitOfWork.VarContacts.Add(new tVarContact
            {
                visible = true,
                VarName = nameoffield,
                colNumber = null,
                colTypeType = fieldtype,
                colTypeFilter = "multipleotpion"
            });

            _unitOfWork.Complete();
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
        







    }
}