using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Feedle.Models
{
    public class UserInformation
    {
        [Key] [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("username")] public string UserName { get; set; }
    }
}