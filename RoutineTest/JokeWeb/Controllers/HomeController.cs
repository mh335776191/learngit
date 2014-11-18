using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationData.DAL;

namespace JokeWeb.Controllers
{
    public class HomeController : Controller
    {
        private IDAL Dal = new QiuBaiDal();
        private const int PAGESIZE = 20;
        public ActionResult Index(int? index)
        {
            //var list = Dal.GetDisPlayList(!index.HasValue ? 1 : index.Value, PAGESIZE);
            //return View(list);
            return View("");
        }

        public JsonResult _list(int index)
        {
            var list = Dal.GetDisPlayList(index, PAGESIZE);
            if (list.Count == 0)
                return Json("-1");
            return Json(list);
        }
    }
}
