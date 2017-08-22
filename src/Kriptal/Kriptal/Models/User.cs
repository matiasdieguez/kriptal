using Realms;

namespace Kriptal.Models
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }

        public string PublicKey { get; set; }
    }
}
