using System.Collections.Generic;

namespace Import_Contacts_From_Json
{
    public class RootObject
    {
        public string Name { get; set; }

        public List<string> Phones { get; set; }

        public List<string> Emails { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public string Site { get; set; }

        public string Notes { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
