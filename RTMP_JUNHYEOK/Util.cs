using System;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RTMP_JUNHYEOK.DAL;
using RTMP_JUNHYEOK.Models;

namespace RTMP_JUNHYEOK
{
    public class Util
    {

        public bool LoginCheck(HttpSessionStateBase Session)
        {
            return Session["loginUser"] != null;
        }

        public bool isValue(string str)
        {
            return (str == null || str.Length == 0) == false;
        }

        public Hashtable getMsgTable(string type, string msg, string action)
        {
            Hashtable msg_options = new Hashtable();
            if (isValue(type) && isValue(msg) && isValue(action))
            {
                msg_options.Add("type", type);
                msg_options.Add("msg", msg);
                msg_options.Add("action", action);
            }
            return msg_options;
        }

    }
}