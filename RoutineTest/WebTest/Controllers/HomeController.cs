using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index(string id)
        {
            Student st = new Student();
            st.Age = 100;
            st.child = null;
            ViewBag.test = "<span>123</span>";
            string url = "http://imguploadcommon.yorhome.com/NewHouse/Premises/UpdateDesc.ashx?guid=ebc73069-ff20-4a3d-be71-316a393711f0&id=10643&desc=sdfsdf&title=bbb&picturetype=Exterior&city=355&callbackparam=success_jsonpCallback&_=1413712663050";
            string ss = test(url);
            return View(st);
        }

        public ActionResult posttest(Student model)
        {
            return Content("dd");
        }
        public static string test(string Url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 5000;
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(Url + ex.Message);
            }
            return "";
        }
        public FileResult testfile()
        {
            NPOI.SS.UserModel.IWorkbook exportFile = new NPOI.XSSF.UserModel.XSSFWorkbook();
            //NPOI.SS.UserModel.ISheet sheet1 = exportFile.CreateSheet("Sheet1");
            //NPOI.SS.UserModel.IRow row = sheet1.CreateRow(0);
            //NPOI.SS.UserModel.ICell cell = row.CreateCell(0);
            //cell.SetCellValue("123");

            //System.IO.FileStream file = new System.IO.FileStream(@"1111.xlsx", System.IO.FileMode.Create);
            MemoryStream stream = new MemoryStream();
            byte[] b = Encoding.UTF8.GetBytes("sffff");
            stream.Write(b, 0, b.Length);
            stream.Flush();
            byte[] b2 = new byte[b.Length + 20];
            stream.Position = 0;
            stream.Read(b2, 0, b2.Length);
            string ss = Encoding.UTF8.GetString(b2);

            stream.Flush();
            stream.Position = 0;
            return File(stream, "application/oknd.ms-excel");

        }

        public ActionResult TestPage()
        {
            return View();
        }

    }
}
