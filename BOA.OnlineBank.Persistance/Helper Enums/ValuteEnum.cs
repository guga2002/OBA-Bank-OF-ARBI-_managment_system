using System.Text.Json.Serialization;

namespace Bank_Managment_System.Helper_Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValuteEnum
    {
        GEl=1,USD=2, EURO=3,
    }
}
