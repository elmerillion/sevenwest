using Newtonsoft.Json;

namespace SevenWestMediaTechInterview.Client.Dto
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("first")]
        public string GivenName { get; set; }

        [JsonProperty("last")]
        public string Surname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

    }
}
