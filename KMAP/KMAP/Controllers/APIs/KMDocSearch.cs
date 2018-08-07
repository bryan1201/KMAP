using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Net;

using System.IO;
using System.Collections;
using System.Collections.Specialized;

using KMAP.Models;
using System.Text;
using KMAP.Controllers.General;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace KMAP.Controllers.APIs
{
    public class KMDocSearchController : ApiController
    {
        private KMDocument kmd;

        /*Example Url:
         * ~/api/KMSearch/KMDocTest
         */
        [HttpGet]
        public HttpResponseMessage KMDocTest()
        {
            var dateNow = new Dictionary<string, string>()
            {
                {"Date", DateTime.Now.ToShortDateString()},
                {"Time", DateTime.Now.ToShortTimeString()}
            };

            string json = JsonConvert.SerializeObject(dateNow, Formatting.Indented);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        [HttpGet]
        public HttpResponseMessage AdvSearchSimple(string advkeyword, string folderId, string userId)
        {
            string json = string.Empty;
            try
            {
                kmd = new KMDocument(userId);
                kmd.AdvSearchSimple(advkeyword, folderId, userId);
                json = JsonConvert.SerializeObject(kmd, Formatting.Indented);
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return result;
        }

        [HttpGet, HttpPost] // 讓此方法可同時接受 HTTP GET 和 POST 請求.
        public HttpResponseMessage AdvSearchDocClass(string docclass, string docclassvalue, string advkeyword, string folderId, string userId)
        {
            string json = string.Empty;
            try
            {
                kmd = new KMDocument(userId);
                kmd.AdvSearchDocClass(docclass, docclassvalue, advkeyword, folderId, userId);
                json = JsonConvert.SerializeObject(kmd, Formatting.Indented);
                //json = kmd.GetResultDocClass(docclass, docclassvalue, advkeyword, folderId, userId);

            }
            catch (Exception ex)
            {
                json = ex.Message;
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return result;
        }
    }
}