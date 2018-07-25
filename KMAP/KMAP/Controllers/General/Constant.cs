using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using KMAP.Models;

namespace KMAP.Controllers.General
{
    public class Constant
    {

        public static string ReadContentUrl = ConfigurationManager.AppSettings["ReadContentUrl"];
        public static String S_SPACE = " ";
        public static String S_Title = ConfigurationManager.AppSettings["Title"];
        public static String NetworkCredentialUserId = ConfigurationManager.AppSettings["NetworkCredentialUserId"];
        public static String NetworkCredentialPWD = ConfigurationManager.AppSettings["NetworkCredentialPWD"];
        
        public static String WebRoot = ConfigurationManager.AppSettings["WebRoot"];
        
        public static String S_ConnStr = "ConnStr";
        public static String DefaultSelect = "-- select one --";
        public static String DefaultSelectAddNew = "-- add new one --";
        public static String DefaultSelectone = "-- select one --";
        public static string LogonUserId = ConfigurationManager.AppSettings["LogonUserId"];
        public static String PRDDBContext = "PRDDBContext";
        public static String QASDBContext = "QASDBContext";
        public static bool IsTracking = bool.Parse(ConfigurationManager.AppSettings["IsTracking"]);
        public static string TrackingSubject = ConfigurationManager.AppSettings["TrackingSubject"];

        public Constant()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
    }
}