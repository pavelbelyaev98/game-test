using System.IO;

namespace SomethingDownThere
{
    public interface IDevicePreferencesStore
    {
        string Read();
        void Write(string contents);
    }

    // Camera and controls share the atomic file policy, never a world checkpoint.
    public class DevicePreferencesFile : IDevicePreferencesStore
    {
        private readonly string path;
        public DevicePreferencesFile(string path) => this.path = path;
        public string Read()
        {
            if (!File.Exists(path)) return null;
            return new FileInfo(path).Length <= 4096 ? File.ReadAllText(path) : null;
        }

        public void Write(string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string pending = path + ".pending";
            using (var stream = new FileStream(pending, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contents);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush(true);
            }
            if (File.Exists(path)) File.Replace(pending, path, null);
            else File.Move(pending, path);
        }
    }
}
