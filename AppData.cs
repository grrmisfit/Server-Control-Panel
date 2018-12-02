// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using DediServerCron;
//
//    var appData = AppData.FromJson(jsonString);
using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace DediServerCron
{


    public partial class AppData
    {
        [JsonProperty("ServerLoc")]
        public string ServerLoc { get; set; }

        [JsonProperty("BatFile")]
        public string BatFile { get; set; }

        [JsonProperty("TimerInterval")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TimerInterval { get; set; }

        [JsonProperty("ServerIp")]
        public string ServerIp { get; set; }

        [JsonProperty("WebToken")]
        public string WebToken { get; set; }

        [JsonProperty("WebPass")]
        public string WebPass { get; set; }
    }

    public partial class AppData
    {
        public static AppData FromJson(string json) => JsonConvert.DeserializeObject<AppData>(json, DediServerCron.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AppData self) => JsonConvert.SerializeObject(self, DediServerCron.Converter.Settings);
    }

    internal static class Converter
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
}
