using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RTMP_JUNHYEOK.DAL;
using RTMP_JUNHYEOK.Models;

namespace RTMP_JUNHYEOK.Controllers
{
    public class RoomController : Controller
    {
        private ChatEntities1 db = new ChatEntities1();
        private Util util = new Util();
        // GET: Room
        public ActionResult Index(string type, string msg, string action)
        {
            Hashtable ht = util.getMsgTable(type, msg, action);
            if (ht.Count > 0) ViewBag.msg = ht;
            User user = Session["loginUser"] as User;
            // 방 번호, 가장 최근 기록 (입장/퇴장)
            Dictionary<int, bool> dict = new Dictionary<int, bool>();

            if (user != null)
            {
                // 로그인 유저의 입장 기록
                IEnumerable<IGrouping<int, EnterHistory>> groups = db.EnterHistory.Where(a => a.user_id == user.Id).GroupBy(a => a.room_id).ToList();

                foreach (var group in groups)
                {
                    if (dict.ContainsKey(group.Key)) continue;
                    dict.Add(group.Key, group.Last().enter);
                }
            }

            ViewBag.room_dict = dict;
            return View(db.Room.OrderByDescending(a => new { a.user_count, a.created_at }).ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (util.LoginCheck(Session) == false)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Room/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하세요. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하세요.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,user_limit")] Room room)
        {
            if (util.LoginCheck(Session) == false)
            {
                ViewBag.msg = util.getMsgTable("error", "로그인 시 사용 가능합니다.", "로그인 필요!");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var user = Session["loginUser"] as User;
                room.user_id = user.Id;
                room.created_at = DateTime.Now;
                room.user_count = 0;
                db.Room.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하세요. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하세요.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,user_limit,max_user_count")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Room.Find(id);
            db.Room.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
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
