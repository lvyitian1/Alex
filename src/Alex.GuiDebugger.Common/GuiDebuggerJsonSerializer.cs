using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Alex.GuiDebugger.Common
{
	public static class GuiDebuggerJsonSerializer
	{
		private static JsonSerializerSettings _settings;
		public static JsonSerializerSettings Settings
		{
			get { return _settings; }
		}

		static GuiDebuggerJsonSerializer()
		{
			_settings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.Objects,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				Culture = CultureInfo.InvariantCulture,
				MissingMemberHandling = MissingMemberHandling.Error,
				ReferenceLoopHandling = ReferenceLoopHandling.Error,
				NullValueHandling = NullValueHandling.Include,
				DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
				MaxDepth = null,
				Formatting = Formatting.None
			};

			_settings.Converters.Add(new GuidJsonConverter());
		}
	}

	internal class GuidJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var guid = (Guid)value;
			serializer.Serialize(writer, guid.ToString("B"));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var guidAsString = serializer.Deserialize<string>(reader);
			return Guid.ParseExact(guidAsString, "B");
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Guid);
		}
	}
}
