using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using KMAP.Controllers.General;
using System.Collections;
using System.Net;
using System.Collections.Specialized;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KMAP.Models
{
    public class KMDocument
    {
        private string DataFormat = KMService.DataFormat;    // (必須)API資料傳輸格式，建議預設使用JSON
        private string kmUserid = Constant.LogonUserId;     // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
        private string tenant = KMService.TENANT;
        private string KMUrl = KMService.KMPUrl;             // (必須)KM Server Site的API虛擬目錄URL路徑

        private string DocClass = string.Empty;
        private string DocClassValue = string.Empty;
        private string AdvKeyword = string.Empty;
        private string FolderId = string.Empty;

        /*BryanHPBook 10.15.69.38*/
        private string API_Key = KMService.API_Key;          // (必須)KM系統中已註冊並啟用的API Key="154e10710ea44cdaaaec9cb4f7910ddc"

        public IList<KMDocDatumClass> KMDocuments { get; set; }
        public IList<KMDocumentFile> KMFiles { get; set; }

        public KMDocument()
        {
            //Do nothing...
        }

        private void SetFileClasses(string userId)
        {
            IList<KMDocumentFile> fileclasses = new List<KMDocumentFile>();
            foreach(var datum in KMDocuments)
            {
                KMDocumentFile kdf = new KMDocumentFile();
                kdf.GetFileClass(docId: datum.UniqueKey.ToString(), userId: userId);
                fileclasses.Add(kdf);
            }
            KMFiles = fileclasses;
        }

        public void AdvSearchSimple(string advkeyword, string folderId, string userId)
        {
            advkeyword = string.IsNullOrEmpty(advkeyword) ? "3D" : advkeyword;
            folderId = string.IsNullOrEmpty(folderId) ? "1573" : folderId;
            kmUserid = string.IsNullOrEmpty(userId) ? kmUserid : userId;
            string jsonString = GetResult(advkeyword, folderId, kmUserid);
            KMDoc kmdoc = KMDoc.FromJson(jsonString);
            KMDocuments = kmdoc.Data.FirstOrDefault().DatumClassArray.ToList();
            SetFileClasses(userId: userId);
        }

        public void AdvSearchDocClass(string docclass, string docclassvalue, string advkeyword, string folderId, string userId)
        {
            docclass = string.IsNullOrEmpty(docclass) ? "23" : docclass;
            docclassvalue = string.IsNullOrEmpty(docclassvalue) ? "摘要:公差分析 AND 作者:Wilson" : docclassvalue;
            advkeyword = string.IsNullOrEmpty(advkeyword) ? "" : advkeyword;
            folderId = string.IsNullOrEmpty(folderId) ? "" : folderId;
            kmUserid = string.IsNullOrEmpty(userId) ? kmUserid : userId;
            string jsonString = GetResultDocClass(docclass, docclassvalue, advkeyword, folderId, kmUserid);
            KMDoc kmdoc = KMDoc.FromJson(jsonString);
            KMDocuments = kmdoc.Data.FirstOrDefault().DatumClassArray.ToList();
            SetFileClasses(userId: userId);
        }

        public string GetResultDocClass(string docclass, string docclassvalue, string advkeyword, string folderId, string userId)
        {
            string result = string.Empty;
            userId = string.IsNullOrEmpty(userId) ? kmUserid : userId;
            advkeyword = string.IsNullOrEmpty(advkeyword) ? string.Empty : advkeyword;
            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetAdvSearchUrl = KMService.GetServiceUrl(ServiceType.GetAdvancedResult, docId:"0", userId: userId, tenant:tenant);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();


            nvc.Add("enabletagsynonyms", "false");
            nvc.Add("enablekeywordsynonyms", "false");
            nvc.Add("containchildcategory", "false");
            nvc.Add("containchildfolder", "true");
            nvc.Add("keywordfield", "摘要");
            nvc.Add("keywordfield", "作者");
            nvc.Add("docclass", docclass);
            nvc.Add("docclassvalue", docclassvalue);

            if (!string.IsNullOrEmpty(advkeyword))
                nvc.Add("keyword", advkeyword.Trim());

            if (!string.IsNullOrEmpty(folderId.Trim()))
                nvc.Add("folder", folderId.Trim());
            
            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));


            byte[] bytesAdvSearch = Encoding.UTF8.GetBytes(uploadData);
            byte[] bytesAdvSearchResult = null;
            string stringAdvSearchResult = "";

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesAdvSearchResult = client2.UploadData(targetAdvSearchUrl, bytesAdvSearch);
                stringAdvSearchResult = Encoding.UTF8.GetString(bytesAdvSearchResult);
                return result = stringAdvSearchResult;
            }
            catch (Exception ex)
            {
                string err = "GetAdvancedResult " + ex.ToString();
                return err;
            }
            finally
            {
                result = stringAdvSearchResult;
            }
        }

        public string GetResult(string advkeyword, string folderId, string userId)
        {
            string result = string.Empty;
            userId = string.IsNullOrEmpty(userId) ? kmUserid : userId;
            advkeyword = string.IsNullOrEmpty(advkeyword) ? "3D" : advkeyword;
            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetAdvSearchUrl = KMService.GetServiceUrl(ServiceType.GetAdvancedResult, docId:"0", userId: userId,
                tenant:tenant);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("enabletagsynonyms", "false");
            nvc.Add("enablekeywordsynonyms", "false");
            nvc.Add("containchildcategory", "false");
            nvc.Add("containchildfolder", "true");
            nvc.Add("keyword", advkeyword.Trim());

            if (folderId.Trim() != string.Empty)
            {
                nvc.Add("folder", folderId.Trim());
            }

            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));


            byte[] bytesAdvSearch = Encoding.UTF8.GetBytes(uploadData);
            byte[] bytesAdvSearchResult = null;
            string stringAdvSearchResult = "";

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesAdvSearchResult = client2.UploadData(targetAdvSearchUrl, bytesAdvSearch);
                stringAdvSearchResult = Encoding.UTF8.GetString(bytesAdvSearchResult);
                return result = stringAdvSearchResult;
            }
            catch (Exception ex)
            {
                string err = "GetAdvancedResult " + ex.ToString();
                return err;
            }
            finally
            {
                result = stringAdvSearchResult;
            }
        }
        
    }

    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var extendedSearchResult = ExtendedSearchResult.FromJson(jsonString);

    public partial class KMDoc
    {
        [JsonProperty("tid")]
        public long Tid { get; set; }

        [JsonProperty("statuscode")]
        public string Statuscode { get; set; }

        [JsonProperty("data")]
        public KMDocDatumUnion[] Data { get; set; }
    }

    public partial class KMDocDatumClass
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("ActivationDatetime")]
        public string ActivationDatetime { get; set; }

        [JsonProperty("Author")]
        public Guid Author { get; set; }

        [JsonProperty("Categories")]
        public object[] Categories { get; set; }

        [JsonProperty("CreationDatetime")]
        public string CreationDatetime { get; set; }

        [JsonProperty("Creator")]
        public Guid Creator { get; set; }

        [JsonProperty("DeactivationDatetime")]
        public string DeactivationDatetime { get; set; }

        [JsonProperty("DocumentClass")]
        [JsonConverter(typeof(KMDocParseStringConverter))]
        public long DocumentClass { get; set; }

        [JsonProperty("Folders")]
        [JsonConverter(typeof(KMDocDecodeArrayConverter))]
        public long[] Folders { get; set; }

        [JsonProperty("LastModifiedDatetime")]
        public string LastModifiedDatetime { get; set; }

        [JsonProperty("Lock")]
        public bool Lock { get; set; }

        [JsonProperty("RelatedItems")]
        public object[] RelatedItems { get; set; }

        [JsonProperty("Score")]
        public double Score { get; set; }

        [JsonProperty("State")]
        public long State { get; set; }

        [JsonProperty("Summary")]
        public string Summary { get; set; }

        [JsonProperty("Tags")]
        public object[] Tags { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("UniqueKey")]
        [JsonConverter(typeof(KMDocParseStringConverter))]
        public long UniqueKey { get; set; }

        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("Fields")]
        public object[] Fields { get; set; }
    }

    public partial struct KMDocDatumUnion
    {
        public KMDocDatumClass[] DatumClassArray;
        public long? Integer;

        public bool IsNull => DatumClassArray == null && Integer == null;
    }

    public partial class KMDoc
    {
        public static KMDoc FromJson(string json) => JsonConvert.DeserializeObject<KMDoc>(json, KMDocConverter.Settings);
    }

    public static class KMDocSerialize
    {
        public static string ToJson(this KMDoc self) => JsonConvert.SerializeObject(self, KMDocConverter.Settings);
    }

    internal static class KMDocConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                KMDocDatumUnionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class KMDocDatumUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(KMDocDatumUnion) || t == typeof(KMDocDatumUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new KMDocDatumUnion { Integer = integerValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<KMDocDatumClass[]>(reader);
                    return new KMDocDatumUnion { DatumClassArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type DatumUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (KMDocDatumUnion)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.DatumClassArray != null)
            {
                serializer.Serialize(writer, value.DatumClassArray);
                return;
            }
            throw new Exception("Cannot marshal type DatumUnion");
        }

        public static readonly KMDocDatumUnionConverter Singleton = new KMDocDatumUnionConverter();
    }

    internal class KMDocParseStringConverter : JsonConverter
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

        public static readonly KMDocParseStringConverter Singleton = new KMDocParseStringConverter();
    }

    internal class KMDocDecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long[]);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = KMDocParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (long[])untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = KMDocParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly KMDocDecodeArrayConverter Singleton = new KMDocDecodeArrayConverter();
    }

    class JsonHelper
    {
        private const string INDENT_STRING = "    ";
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}