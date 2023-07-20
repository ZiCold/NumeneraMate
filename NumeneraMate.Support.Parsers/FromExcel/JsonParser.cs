using System.Text.Encodings.Web;
using System.Text.Json;

namespace NumeneraMate.Support.Parsers.FromExcel
{
    public class JsonParser
    {
        public JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }
    }
}
