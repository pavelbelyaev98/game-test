using UnityEngine.SceneManagement;

namespace SomethingDownThere.Tests
{
    internal sealed class TestInputPreferences : IDevicePreferencesStore
    {
        public string Contents;
        public string Read() => Contents;
        public void Write(string contents) => Contents = contents;
        // Exercise retained save content explicitly now that new games contain only rocks.
        public static void RestoreBottleCompatibilityFixture(DiscoveryField field)
        {
            var saved = field.Capture();
            int i = 0;
            foreach (var entry in field.Catalog.Entries)
            {
                if (!entry.ItemId.StartsWith("common_bottle_")) continue;
                saved[i].ContentId = entry.Prefab.SaveContentId;
                saved[i].Item.Name = entry.Prefab.DisplayName;
                saved[i].Item.Value = entry.Prefab.SaleValue;
                i++;
            }
            NUnit.Framework.Assert.That(i, NUnit.Framework.Is.EqualTo(3));
            field.Restore(saved, field.Seed);
        }
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
