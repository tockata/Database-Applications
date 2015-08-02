namespace Movies_EF_CodeFirst.Migrations
{
    using Newtonsoft.Json;

    internal class UsersMoviesDto
    {
        public string Username { get; set; }

        [JsonProperty("favouriteMovies")]
        public virtual string[] Isbn { get; set; }
    }
}
