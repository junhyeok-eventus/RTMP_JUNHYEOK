using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RTMP_JUNHYEOK.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RTMP_JUNHYEOK
{
    public class ChatHub : Hub
    {
        private ChatEntities1 db = new ChatEntities1();

        public async Task JoinRoom(int user_id, int room_id)
        {
            Room room = getRoom(room_id);
            User user = getUser(user_id);
            EnterHistory recent_history = db.EnterHistory
                                            .Where(a => a.user_id == user_id && a.room_id == room_id)
                                            .OrderByDescending(a => a.created_at)
                                            .FirstOrDefault();

            await Groups.Add(Context.ConnectionId, room.Id.ToString());

            // 나간 기록이 있다면 입장 알림
            if (recent_history == null || recent_history.enter == false)
            {
                UserEnter(user, room, true);
                SendGroup(room_id, user_id, user.hello_message);
            }
        }

        public void LeaveRoom(int user_id, int room_id)
        {
            Room room = getRoom(room_id);
            User user = getUser(user_id);
            UserEnter(user, room, false);
            SendGroup(room_id, user_id, user.name + " 님이 퇴장 했습니다.");
            Groups.Remove(Context.ConnectionId, room.Id.ToString());
        }

        private void UserEnter(User user, Room room, bool enter)
        {
            // 입장 기록 저장
            EnterHistory enterHistory = new EnterHistory { User = user, Room = room, enter = enter, created_at = DateTime.Now };
            db.EnterHistory.Add(enterHistory);
            
            // 입장 여부에 따라 user_count 변경
            room.user_count += enter ? 1 : -1;
            room.user_count = room.user_count < 0 ? 0 : room.user_count > room.user_limit ? room.user_limit : room.user_count;
            db.Entry(room).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendGroup(int room_id, int user_id, string message)
        {
            Room room = getRoom(room_id);
            User user = getUser(user_id);
            Message msg = new Message { User = user, Room = room, content = message, delete = false, time = DateTime.Now};
            Clients.Group(room_id.ToString()).broadcastMessage(new { name = msg.User.name, content = msg.content, delete = msg.delete });
            db.Message.Add(msg);
            db.SaveChanges();
        }

        public IEnumerable<dynamic> GetData(int room_id)
        {
            return db.Message.Where(a => a.room_id == room_id).OrderBy(a => a.time).Select(a => new { a.User.name, a.content, a.delete }).ToList();
        }

        private Room getRoom(int room_id)
        {
            return db.Room.Find(room_id);
        }

        private User getUser(int user_id)
        {
            return db.User.Find(user_id);
        }
    }
}