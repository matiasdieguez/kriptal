namespace Kriptal.Models
{
    public class KriptalMessage
    {
        public string TextData { get; set; }
        public string TextAesKey { get; set; }
        public string TextAesIv { get; set; }
        public string Signature { get; set; }
        public string FromId { get; set; }
        public string FileData { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileAesKey { get; set; }
        public string FileAesIv { get; set; }
    }
}
