using System;
using Newtonsoft.Json;

namespace Alex.GuiDebugger.Common.Serialization.JsonConverters
{
    internal class GuidJsonConverter : JsonConverter<Guid>
    {
        private const string GuidFormat = "B";
        
        public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString(GuidFormat));
        }

        public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var guidAsString = serializer.Deserialize<string>(reader);
            return Guid.ParseExact(guidAsString, GuidFormat);
        }
    }
}