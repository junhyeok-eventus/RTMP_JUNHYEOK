using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RTMP_JUNHYEOK.Models;

namespace RTMP_JUNHYEOK.DAL
{
    public class ChatInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ChatEntities1>
    {
        protected override void Seed(ChatEntities1 context)
        {
            var users = new List<User>
            {
                new User{name="admin", password="1234"},
                new User{name="test", password="1234"},
            };

            int room_idx = 1, message_idx = 1;
            users.ForEach(user => {
                context.User.Add(user);
                context.Room.Add(new Room { Id = room_idx++, user_count = 1, user_limit = 4 });
                context.Message.Add(new Message { Id = message_idx++, user_id = user.Id, room_id = room_idx - 1, delete = false, content = user.name + " 님이 방을 개설하였습니다.", time = DateTime.Now});
                context.Message.Add(new Message { Id = message_idx++, user_id = user.Id, room_id = room_idx - 1, delete = false, content = user.name + " 님이 방에 입장하였습니다.", time = DateTime.Now });
                if (user.hello_message != null && user.hello_message != "")
                {
                    context.Message.Add(new Message { Id = message_idx++, user_id = user.Id, room_id = room_idx - 1, delete = false, content = user.hello_message, time = DateTime.Now });
                }
            });
            context.SaveChanges();
        }
    }
}