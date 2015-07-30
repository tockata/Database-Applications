namespace NewsDatabase
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class NewsContext : DbContext
    {
        // Your context has been configured to use a 'NewsContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'NewsDatabase.NewsContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'NewsContext' 
        // connection string in the application configuration file.
        public NewsContext()
            : base("name=NewsContext")
        {
        }

        public IDbSet<News> News { get; set; }
    }
}