namespace Kriptal.Services
{
    public interface ISender
    {
        void Send(string text, string to);
        void Send(string text, string to, byte[] file, string fileName);
        string GetFilePath(string fileName);
        byte[] GetFileData(string path);
        void SaveFile(string path, byte[] file);
    }
}
