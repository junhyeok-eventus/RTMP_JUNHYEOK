using RTMP_JUNHYEOK.Models;
using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTMP_JUNHYEOK.Controllers
{
    public class HomeController : Controller
    {
        private ChatEntities1 db = new ChatEntities1();
        private Util util = new Util();
        public ActionResult Index(string type, string msg, string action)
        {
            ViewBag.msg = util.getMsgTable(type, msg, action);
            
            return View();
        }

        public ActionResult Login(string msg)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string name, string password)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = new { success = false, error = true };
            if (util.isValue(name) && util.isValue(password))
            {
                var user = db.User.Where(a => a.name == name && a.password == password).FirstOrDefault();
                if (user != null)
                {
                    jsonResult.Data = new { success = true, error = false, name = user.name };
                    Session["loginUser"] = user;
                }
            }
            return jsonResult;
        }

        public ActionResult Logout(string name, string password)
        {
            Session["loginUser"] = null;
            return Redirect("~/User/Index");
        }
    }
}