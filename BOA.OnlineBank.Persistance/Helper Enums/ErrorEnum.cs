using System.Text.Json.Serialization;

namespace ErrorEnumi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorEnum
    {
        DEBUG=1, INFO=2, WARNING=3, ERROR=4, FATAL=5
    }
}