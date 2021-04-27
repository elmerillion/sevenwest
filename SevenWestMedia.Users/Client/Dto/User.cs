using System.Text.Json.Serialization;

namespace SevenWestMediaTechInterview.Client.Dto
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("age")]
        public int Age { get; set; }
        [JsonPropertyName("first")]
        public string GivenName { get; set; }
        [JsonPropertyName("last")]
        public string Surname { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

    }
}
