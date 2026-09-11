using UnityEngine.SceneManagement;

namespace SomethingDownThere.Tests
{
    internal sealed class TestInputPreferences : IDevicePreferencesStore
    {
        public string Contents;
        public string Read() => Contents;
        public void Write(string contents) => Contents = contents;
        public static void Configure(Scene scene, LoadSceneMode mode)
        {
            foreach (var root in scene.GetRootGameObjects())
                foreach (var player in root.GetComponentsInChildren<FpsPlayer>(true))
                {
                    player.ConfigureInputPreferences(new TestInputPreferences());
                    player.ConfigureGamePreferences(new TestInputPreferences());
                }
        }
    }
}
