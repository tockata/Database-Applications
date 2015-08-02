namespace Movies_EF_CodeFirst.Migrations
{
    using Newtonsoft.Json;

    internal class MovieRatingsDto
    {
        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("movie")]
        public string Isbn { get; set; }

        public int Rating { get; set; }
    }
}
