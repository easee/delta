﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Billing.Api.Helpers
{
    public class LogHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Log(string Message, string Level = "ERROR")
        {
            if (HttpContext.Current != null) Message += ": " + HttpContext.Current.Request.Url.AbsoluteUri;
            if (Level == "INFO") log.Info(Message); else log.Error(Message);

            //if (!url.ToLower().Contains("localhost")) Email.Send(“admin@billing.com", “>> " + Level, Message);
        }
    }
}