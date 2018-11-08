namespace Kriptal.Models
{
    public class BlockhainStamp
    {
        public KriptalMessage Message { get; set; }
        public string SenderPublicKey { get; set; }
        public string DestinationPublicKey { get; set; }
        public string DateTime { get; set; }
    }
}