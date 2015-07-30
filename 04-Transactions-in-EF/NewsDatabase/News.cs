namespace NewsDatabase
{
    using System.ComponentModel.DataAnnotations;

    public class News
    {
        public int Id { get; set; }

        [ConcurrencyCheck]
        public string Content { get; set; }
    }
}
