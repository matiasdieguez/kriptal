using Realms;

namespace Kriptal.Models
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string PublicKey { get; set; }
    }

    public class UserItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PublicKey { get; set; }
    }

}
