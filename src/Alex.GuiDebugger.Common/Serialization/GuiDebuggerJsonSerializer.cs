using System.Globalization;
using Alex.GuiDebugger.Common.Serialization.JsonConverters;
using Newtonsoft.Json;

namespace Alex.GuiDebugger.Common.Serialization
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
}