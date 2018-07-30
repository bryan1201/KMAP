using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jayrock.Json;
using System.Net;
using Jayrock.Json.Conversion;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using KMAP.Models;
using System.Text;
using KMAP.Controllers.General;

namespace KMAP.Controllers.APIs
{
    public class AdvSearch
    {
        private KMDocument kmd;
        
        public IList<KMDocDatumClass> datumClasses { get; set; }

        public AdvSearch()
        {

        }

        public string AdvSearchSimple(string advkeyword, string folderId, string userId)
        {
            string result = string.Empty;
            try
            {
                kmd = new KMDocument();
                result = kmd.GetResult(advkeyword, folderId, userId);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string AdvSearchDocClass(string docclass, string docclassvalue, string advkeyword, string folderId, string userId)
        {
            string result = string.Empty;
            try
            {
                kmd = new KMDocument();
                kmd.AdvSearchDocClass(docclass, docclassvalue, advkeyword, folderId, userId);
                result = kmd.GetResultDocClass(docclass, docclassvalue, advkeyword, folderId, userId);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}