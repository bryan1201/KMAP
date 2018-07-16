using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KMAP.Models
{ 
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var extendedSearchResult = ExtendedSearchResult.FromJson(jsonString);

        public partial class ExtendedSearchResult
        {
            [JsonProperty("tid")]
            public long Tid { get; set; }

            [JsonProperty("statuscode")]
            public string Statuscode { get; set; }

            [JsonProperty("data")]
            public DatumUnion[] Data { get; set; }
        }

        public partial class DatumClass
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
            [JsonConverter(typeof(ParseStringConverter))]
            public long DocumentClass { get; set; }

            [JsonProperty("Folders")]
            [JsonConverter(typeof(DecodeArrayConverter))]
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
            [JsonConverter(typeof(ParseStringConverter))]
            public long UniqueKey { get; set; }

            [JsonProperty("Version")]
            public long Version { get; set; }

            [JsonProperty("Fields")]
            public object[] Fields { get; set; }
        }

        public partial struct DatumUnion
        {
            public DatumClass[] DatumClassArray;
            public long? Integer;

            public bool IsNull => DatumClassArray == null && Integer == null;
        }

        public partial class ExtendedSearchResult
        {
            public static ExtendedSearchResult FromJson(string json) => JsonConvert.DeserializeObject<ExtendedSearchResult>(json, Converter.Settings);
        }

        public static class Serialize
        {
            public static string ToJson(this ExtendedSearchResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = {
                DatumUnionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class DatumUnionConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(DatumUnion) || t == typeof(DatumUnion?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                        var integerValue = serializer.Deserialize<long>(reader);
                        return new DatumUnion { Integer = integerValue };
                    case JsonToken.StartArray:
                        var arrayValue = serializer.Deserialize<DatumClass[]>(reader);
                        return new DatumUnion { DatumClassArray = arrayValue };
                }
                throw new Exception("Cannot unmarshal type DatumUnion");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (DatumUnion)untypedValue;
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

            public static readonly DatumUnionConverter Singleton = new DatumUnionConverter();
        }

        internal class ParseStringConverter : JsonConverter
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

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }

        internal class DecodeArrayConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long[]);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                reader.Read();
                var value = new List<long>();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    var converter = ParseStringConverter.Singleton;
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
                    var converter = ParseStringConverter.Singleton;
                    converter.WriteJson(writer, arrayItem, serializer);
                }
                writer.WriteEndArray();
                return;
            }

            public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
        }

}