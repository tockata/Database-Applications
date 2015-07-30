namespace ProductShop.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using ProductShop.Models;

    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
            : base("name=ProductShopContext")
        {
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("FriendId");
                    m.ToTable("UserFriends");
                });

            modelBuilder.Entity<Product>()
                .HasOptional<User>(p => p.Buyer)
                .WithMany(b => b.BoughtProducts)
                .HasForeignKey(p => p.BuyerId);

            modelBuilder.Entity<Product>()
                .HasRequired<User>(p => p.Seller)
                .WithMany(b => b.SoldProducts)
                .HasForeignKey(p => p.SellerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}