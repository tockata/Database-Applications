namespace EF_Performance
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;

    public class AdsTest
    {
        public static void Main()
        {
            var context = new AdsEntities();
            Stopwatch sw = new Stopwatch();

            // Problem 1.Show Data from Related Tables

            // Slow version /uncomment to test/:
            var adsWithoutInclude = context.Ads;
            foreach (var ad in adsWithoutInclude)
            {
                var title = ad.Title ?? "no title";
                var status = ad.AdStatuses != null ? ad.AdStatuses.Status : "no status";
                var category = ad.Categories != null ? ad.Categories.Name : "no category";
                var town = ad.Towns != null ? ad.Towns.Name : "no town";
                var user = ad.AspNetUsers != null ? ad.AspNetUsers.UserName : "no user";

                Console.WriteLine(
                    "Title: {0}, status: {1}, category: {2}, town: {3}, user: {4}",
                    title,
                    status,
                    category,
                    town,
                    user);
            }

            // Optimized version /uncomment to test/:
            //var adsWithInclude = context.Ads
            //    .Include(a => a.AdStatuses)
            //    .Include(a => a.Categories)
            //    .Include(a => a.Towns)
            //    .Include(a => a.AspNetUsers);

            //foreach (var ad in adsWithInclude)
            //{
            //    var title = ad.Title ?? "no title";
            //    var status = ad.AdStatuses != null ? ad.AdStatuses.Status : "no status";
            //    var category = ad.Categories != null ? ad.Categories.Name : "no category";
            //    var town = ad.Towns != null ? ad.Towns.Name : "no town";
            //    var user = ad.AspNetUsers != null ? ad.AspNetUsers.UserName : "no user";

            //    Console.WriteLine(
            //        "Title: {0}, status: {1}, category: {2}, town: {3}, user: {4}",
            //        title,
            //        status,
            //        category,
            //        town,
            //        user);
            //}

            // Problem 2.Play with ToList()

            // Slow version /uncomment to test/:

            //context.Database.ExecuteSqlCommand("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

            //sw.Start();
            //var adsToListNonOptimized = context.Ads
            //    .ToList()
            //    .OrderBy(a => a.Date)
            //    .Where(a => a.Categories != null && a.AdStatuses.Status == "Published")
            //    .Select(a => new
            //        {
            //            a.Title,
            //            Category = a.Categories != null ? a.Categories.Name : "no category",
            //            Town = a.Towns.Name
            //        })
            //    .ToList();
            //Console.WriteLine(sw.Elapsed);

            //// Optimized version /uncomment to test/:
            //sw.Restart();
            //var adsToListOptimized = context.Ads
            //    .OrderBy(a => a.Date)
            //    .Where(a => a.Categories != null && a.AdStatuses.Status == "Published")
            //    .Select(a => new
            //    {
            //        a.Title,
            //        Category = a.Categories != null ? a.Categories.Name : "no category",
            //        Town = a.Towns.Name
            //    })
            //    .ToList();
            //Console.WriteLine(sw.Elapsed);

            // Problem 3.Select Everything vs. Select Certain Columns
            
            // slow version /uncomment to test/:
        //    context.Database.ExecuteSqlCommand("CHECKPOINT; DBCC DROPCLEANBUFFERS;");
        //    sw.Start();
        //    var adsSelectStar = context.Ads;
        //    foreach (var ad in adsSelectStar)
        //    {
        //        Console.WriteLine(ad.Title);
        //    }

        //    Console.WriteLine(sw.Elapsed);

        //    sw.Restart();
        //    var adsSelectTitle = context.Ads.Select(a => a.Title);
        //    foreach (var title in adsSelectTitle)
        //    {
        //        Console.WriteLine(title);
        //    }

        //    Console.WriteLine(sw.Elapsed);
        }
    }
}
