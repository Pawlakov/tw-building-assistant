namespace TWBuildingAssistant.Data.Json
{
    using Newtonsoft.Json.Converters;

    public class JsonEnumConverter : StringEnumConverter
    {
        public JsonEnumConverter()
        {
            this.AllowIntegerValues = false;
        }
    }
}