using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using KMAP.Controllers.General;
using System.Collections;
using System.Net;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace KMAP.Models
{
    
    public class KMDocumentFile
    {
        private string kmUserid = Constant.LogonUserId;     // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
        private string tenant = KMService.TENANT;
        public IList<KFDatum> kmFiles { get; set; }
        public KMDocumentFile()
        {

        }

        public string GetFileResult(string docId)
        {
            string result = string.Empty;

            return result;
        }

        public string GetFileClass(string docId, string userId)
        {
            string result = string.Empty;
            userId = string.IsNullOrEmpty(userId) ? kmUserid : userId;
            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetAdvSearchUrl = KMService.GetServiceUrl(ServiceType.GetDocumentFileById, docId: docId, userId: userId, tenant: tenant);

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                result = client2.DownloadString(targetAdvSearchUrl);
                //ExtendedSearchResult extendedSearchResult = ExtendedSearchResult.FromJson(result);
                KMFile kmfileSearchResult = KMFile.FromJson(result);
                kmFiles = kmfileSearchResult.Data;

                
                
                return result;
            }
            catch (Exception ex)
            {
                string err = "GetDocumentFileById " + ex.ToString();
                return err;
            }
            finally
            {
                //
            }
        }
    }

    public partial class KMFile
    {
        [JsonProperty("tid")]
        public long Tid { get; set; }

        [JsonProperty("statuscode")]
        public string Statuscode { get; set; }

        [JsonProperty("data")]
        public KFDatum[] Data { get; set; }
    }

    public partial class KFDatum
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("CurrentPath")]
        public string CurrentPath { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("FileSize")]
        public long FileSize { get; set; }

        [JsonProperty("Ocr")]
        public bool Ocr { get; set; }

        [JsonProperty("OcrLanguage")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OcrLanguage { get; set; }

        [JsonProperty("SourceUri")]
        public string SourceUri { get; set; }
    }

    public partial class KMFile
    {
        public static KMFile FromJson(string json) => JsonConvert.DeserializeObject<KMFile>(json, KMAP.Models.KFConverter.Settings);
    }

    public static class KFSerialize
    {
        public static string ToJson(this KMFile self) => JsonConvert.SerializeObject(self, KMAP.Models.KFConverter.Settings);
    }

    internal static class KFConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class KFParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly KFParseStringConverter Singleton = new KFParseStringConverter();
    }
}