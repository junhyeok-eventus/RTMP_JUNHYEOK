using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RTMP_JUNHYEOK.Models;

namespace RTMP_JUNHYEOK.Controllers
{
    public class EnterHistoriesController : Controller
    {
        private ChatEntities1 db = new ChatEntities1();

        // GET: EnterHistories
        public ActionResult Index()
        {
            var enterHistory = db.EnterHistory.Include(e => e.Room).Include(e => e.User);
            return View(enterHistory.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
