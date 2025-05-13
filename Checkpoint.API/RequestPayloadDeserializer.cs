using System.Text.Json;

namespace Checkpoint.API
{
    public static class RequestPayloadDeserializer
    {
        public static void ParseJsonElementValue(JsonElement jsonElement, out object? data)
        {
            data = jsonElement.ValueKind switch
            {
                JsonValueKind.String => jsonElement.GetString(),
                JsonValueKind.Number => jsonElement.TryGetInt32(out int int16) ? int16 :
                                        jsonElement.TryGetInt16(out short shortValue) ? shortValue :
                                        jsonElement.TryGetInt64(out long longValue) ? longValue :
                                        throw new Exception("Hata"),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => throw new Exception("Tanımlanamayan tip")
            };

        }
    }
}
