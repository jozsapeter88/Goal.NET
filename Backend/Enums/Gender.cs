using System.Text.Json.Serialization;

namespace Backend.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}