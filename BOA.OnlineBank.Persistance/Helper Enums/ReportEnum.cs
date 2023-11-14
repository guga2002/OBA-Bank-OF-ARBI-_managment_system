using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Bank_Managment_System.Helper_Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReportEnum
    {
        [EnumMember(Value = "One Month")]
        oneMonth = 1,

        [EnumMember(Value = "Six Months")]
        sixMonth = 6,

        [EnumMember(Value = "One Year")]
        OneYear = 12
    }
}
