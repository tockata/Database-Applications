namespace Diablo_EF_DatabaseFirst
{
    using System;
    using System.Linq;

    public class TestCass
    {
        public static void Main()
        {
            var context = new DiabloEntities();
            var charactersNames = context.Characters
                .Select(c => c.Name);

            foreach (var charactersName in charactersNames)
            {
                Console.WriteLine(charactersName);
            }
        }
    }
}
