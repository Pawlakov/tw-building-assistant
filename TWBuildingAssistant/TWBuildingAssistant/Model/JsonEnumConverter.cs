namespace TWBuildingAssistant.Model
{
    using System;

    using EnumsNET;

    using Newtonsoft.Json;

    public class JsonEnumConverter<TEnum> : JsonConverter where TEnum : struct, Enum
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var asString = serializer.Deserialize<string>(reader);
            return Enums.Parse<TEnum>(asString, EnumFormat.Name);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}