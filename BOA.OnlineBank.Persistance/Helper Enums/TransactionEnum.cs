using System.Text.Json.Serialization;

namespace Bank_Managment_System.Helper_Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionEnum
    {
        piradi=1,
        sxvastan=2,
        tanxisGatana=3

    }
}
