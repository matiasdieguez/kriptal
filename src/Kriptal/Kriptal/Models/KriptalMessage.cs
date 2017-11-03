namespace Kriptal.Models
{
    public class KriptalMessage
    {
        public string Data { get; set; }
        public string AesKey { get; set; }
        public string AesIv { get; set; }
        public string Signature { get; set; }
        public string FromId { get; set; }
    }
}
