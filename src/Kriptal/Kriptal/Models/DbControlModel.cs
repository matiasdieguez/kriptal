using Realms;

namespace Kriptal.Models
{
    public class DbControlModel : RealmObject
    {
        [PrimaryKey]
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public enum DbControlKeys
    {
        DatabaseId,
        MyPublicKey,
        MyPrivateKey,
        SaltBytes,
        MyName,
        MyId,
        Email
    }
}